using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Writing.Configurations;
using Writing.Entities;
using Writing.Payloads;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;

namespace Writing.Services.Impl; 

public class AuthServiceImpl : AuthService {

    private readonly DataContext dataContext;
    private readonly UserConverter userConverter;
    private readonly ResponseObject<UserDTO> responseObject;
    private readonly ResponseObject<ResponseTokenObject> responseToken;
    private readonly RefreshTokenService refreshTokenService;
    private readonly SecurityConfiguration securityConfiguration;
    private readonly IConfiguration configuration;
    private readonly SendEmailService sendEmailService;
    private readonly IDistributedCache distributedCache;

    private static readonly long refreshTokenExpireOnMs = 604800000L; //7day

    public AuthServiceImpl(DataContext dataContext, UserConverter userConverter, ResponseObject<UserDTO> responseObject, 
        ResponseObject<ResponseTokenObject> responseToken, RefreshTokenService refreshTokenService, 
        SecurityConfiguration securityConfiguration, IConfiguration configuration, SendEmailService sendEmailService,
        IDistributedCache distributedCache) {
        this.dataContext = dataContext;
        this.userConverter = userConverter;
        this.responseObject = responseObject;
        this.responseToken = responseToken;
        this.refreshTokenService = refreshTokenService;
        this.securityConfiguration = securityConfiguration;
        this.configuration = configuration;
        this.sendEmailService = sendEmailService;
        this.distributedCache = distributedCache;
    }

    public async Task<ResponseObject<UserDTO>> register(RegisterRequest request) {
        User userExisted = dataContext.Users.Where(user => user.Email.Equals(request.Email)).FirstOrDefault();
        if (userExisted != null) {
            return responseObject.responseError(StatusCodes.Status400BadRequest,
                "Email has been registered an account", null);
        }
        User user = userConverter.regRequestToEntity(request);
        user.AvatarPhoto = "resource%2Fimages%2Fdefault%2Fdefault_avt.png";
        user.CoverPhoto = "resource%2Fimages%2Fdefault%2Fdefault_cover.png";
        user.IsActive = false;
        dataContext.Users.Add(user);
        dataContext.SaveChanges();
        
        refreshTokenService.initBlank(user);

        int activeKey = generateActiveKey();
        string message = await sendEmailService.sendEmail(new EmailTo {
            To = request.Email,
            Subject = "Kích hoạt tài khoản tại Writing Dotnet project. ",
            Content = $"Mã kích hoạt tài khoản của bạn là: {activeKey} \nMã có hiệu lực trong 24h. Nếu không kích hoạt, tài khoản của bạn sẽ bị vô hiệu hóa!"
        });

        cachingActiveKey(user.Id, activeKey);

        return responseObject.responseSuccess(message, userConverter.entityToDto(user));
    }

    private void cachingActiveKey(int userId, int activeKey) {
        distributedCache.SetString(userId.ToString(), activeKey.ToString());
    }

    private int generateActiveKey() {
        Random random = new Random();
        return random.Next(1000, 9999);
    }

    public ResponseObject<ResponseTokenObject> login(LoginRequest request) {
        User user = dataContext.Users.Where(user => user.Email.Equals(request.Email)).FirstOrDefault();
        
        if (user == null) {
            return responseToken.responseError(StatusCodes.Status401Unauthorized,
                "User not found with email '" + request.Email + "'", null);
        }
        
        string passwordEncoded = securityConfiguration.encodePassword(request.Password, user.Salt);
        if (!passwordEncoded.Equals(user.Password)) {
            return responseToken.responseError(StatusCodes.Status401Unauthorized,
                "Incorrect password", null);
        }

        string accessToken = generateAccessToken(user);
        RefreshToken refreshToken = refreshTokenService.renewRefreshToken(user.Id);

        if (refreshToken == null) {
            return responseToken.responseError(StatusCodes.Status401Unauthorized,
                "Unrecognize refresh token", null);
        }

        return responseToken.responseSuccess("Success", new ResponseTokenObject {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            ExpireDate = refreshToken.ExpireDate
        });
    }

    public ResponseObject<ResponseTokenObject> renewAccessToken(TokenObjectRequest request) {
        RefreshToken refreshToken = dataContext.RefreshTokens
            .Where(refreshToken => refreshToken.Token.Equals(request.RefreshToken))
            .FirstOrDefault();

        if (refreshToken == null) {
            responseToken.responseError(StatusCodes.Status401Unauthorized, 
                "Unrecognize refresh token", null);
        }

        if (refreshToken.ExpireDate < DateTime.Now) {
            responseToken.responseError(StatusCodes.Status401Unauthorized, 
                "Refresh token has expired", null);
        }

        return responseToken.responseSuccess("Success", new ResponseTokenObject {
            AccessToken = generateAccessToken(refreshToken.User),
            RefreshToken = refreshToken.Token,
            ExpireDate = refreshToken.ExpireDate
        });
    }

    private string generateAccessToken(User user) {
        List<Claim> claims = new List<Claim> {
            new Claim("Id", user.Id.ToString()),
            new Claim("Email", user.Email),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim("Gender", user.Gender),
            new Claim("Role", user.Role),
            new Claim("IsActive", user.IsActive.ToString())
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            configuration.GetSection("Jwt:Secret-key").Value!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(23),
            signingCredentials: credentials);
        
        return new JwtSecurityTokenHandler().WriteToken(token);
        
    }
}