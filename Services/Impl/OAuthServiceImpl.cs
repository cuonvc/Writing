using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using Writing.Entities;
using Writing.Payloads.Converters;
using Writing.Payloads.Responses;
using System.Text.Json;
using Writing.Repositories;

namespace Writing.Services.Impl;

public class OAuthServiceImpl : OAuthService {

    private readonly ResponseObject<ResponseTokenObject> responseObject;
    private readonly ILogger<OAuthServiceImpl> logger;
    private readonly RefreshTokenService refreshTokenService;
    private readonly AuthService authService;
    private readonly DataContext dataContext;
    private readonly ResponseObject<ResponseTokenObject> responseToken;

    public OAuthServiceImpl(ResponseObject<ResponseTokenObject> responseObject, ILogger<OAuthServiceImpl> logger,
        RefreshTokenService refreshTokenService, DataContext dataContext, AuthService authService,
        ResponseObject<ResponseTokenObject> responseToken) {
        this.responseObject = responseObject;
        this.logger = logger;
        this.refreshTokenService = refreshTokenService;
        this.dataContext = dataContext;
        this.authService = authService;
        this.responseToken = responseToken;
    }

    public async Task<ResponseObject<ResponseTokenObject>> validateGoogleToken(string token) {

        try {
            string googleApiInfo = "https://www.googleapis.com/oauth2/v3/userinfo";
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await client.GetAsync(googleApiInfo);
            if (response.IsSuccessStatusCode) {
                string responseJson = await response.Content.ReadAsStringAsync();
                GoogleResponse customObject = JsonSerializer.Deserialize<GoogleResponse>(responseJson);

                User user = dataContext.Users.Where(user => user.Email == customObject.email).FirstOrDefault();
                logger.LogInformation("aaaaaaaa - " + customObject.email);
                if (user == null) {
                    user = new User();
                    user.Email = customObject.email;
                    user.CreatedDate = DateTime.Now;
                    user.IsActive = true;
                    user.Password = "";
                    user.FirstName = "";
                    user.LastName = "";
                    user.Salt = new byte[8];
                    dataContext.Users.Add(user);
                    dataContext.SaveChanges();

                    refreshTokenService.initBlank(user);
                }

                user.AvatarPhoto = customObject.picture;
                user.FirstName = customObject.given_name;
                user.LastName = customObject.family_name;
                dataContext.SaveChanges();
                string accessToken = authService.generateAccessToken(user);
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

            } else {
                Console.WriteLine("Request failed with status code: " + response.StatusCode);
                return responseObject.responseError(StatusCodes.Status401Unauthorized, "Error", null);
            }
        } catch (Exception e) {
            Console.WriteLine("Error - ", e.Message);
            return responseObject.responseError(StatusCodes.Status400BadRequest, e.Message, null);
        }
    }
}
