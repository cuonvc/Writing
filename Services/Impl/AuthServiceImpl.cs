using Microsoft.EntityFrameworkCore;
using Writing.Entities;
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

    public AuthServiceImpl(DataContext dbContext, UserConverter userConverter, ResponseObject<UserDTO> responseObject) {
        this.dataContext = dbContext;
        this.userConverter = userConverter;
        this.responseObject = responseObject;
    }

    public ResponseObject<UserDTO> register(RegisterRequest request) {
        User user = userConverter.regRequestToEntity(request);
        dataContext.Users.Add(user);
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Register successfully", userConverter.entityToDto(user));
    }
}