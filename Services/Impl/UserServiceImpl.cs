﻿using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Writing.Configurations;
using Writing.Entities;
using Writing.Handle;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;
using Role = Writing.Enumerates.Role;

namespace Writing.Services.Impl; 

public class UserServiceImpl : UserService {

    private readonly DataContext dataContext;
    private readonly ResponseObject<UserDTO> responseObject;
    private readonly ResponseObject<string> responseString;
    private readonly ResponseObject<List<UserDTO>> responseList;
    private readonly UserConverter userConverter;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly SecurityConfiguration securityConfiguration;
    private readonly FileHandler fileHandler;
    private readonly IDistributedCache distributedCache;

    public UserServiceImpl(DataContext dataContext, ResponseObject<UserDTO> responseObject,
        ResponseObject<List<UserDTO>> responseList, UserConverter userConverter,
        IHttpContextAccessor httpContextAccessor, ResponseObject<string> responseString,
        SecurityConfiguration securityConfiguration, FileHandler fileHandler,
        IDistributedCache distributedCache) {
        this.dataContext = dataContext;
        this.responseObject = responseObject;
        this.responseList = responseList;
        this.userConverter = userConverter;
        this.httpContextAccessor = httpContextAccessor;
        this.responseString = responseString;
        this.securityConfiguration = securityConfiguration;
        this.fileHandler = fileHandler;
        this.distributedCache = distributedCache;
    }

    public ResponseObject<string> active(int userId, int activeCode) {
        User user = dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefault();

        // if (user == null) {
        //     return responseString.responseError(StatusCodes.Status400BadRequest, 
        //         "User not found with ID: " + userId, null);
        // }

        byte[] value = distributedCache.Get(userId.ToString());

        if (value.IsNullOrEmpty()) {
            return responseString.responseError(StatusCodes.Status400BadRequest,
                "Key not found", null);
        }
        if (!distributedCache.GetString(userId.ToString()).Equals(activeCode.ToString())) {
            return responseString.responseError(StatusCodes.Status400BadRequest,
                "Active code đã hết hạn hoặc không đúng", null);
        }

        user.IsActive = true;
        dataContext.SaveChanges();
        distributedCache.Remove(userId.ToString());

        return responseString.responseSuccess("Success", "Kích hoạt tài khoản thành công!");
    }

    public ResponseObject<UserDTO> getByEmail(string email) {
        User user = dataContext.Users.Where(user => user.Email.Equals(email)).FirstOrDefault();

        if (user == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "User not found with username: " + email, null);
        }

        return responseObject.responseSuccess("Success", userConverter.entityToDto(user));
    }

    public ResponseObject<List<UserDTO>> getAll(int pageNo, int pageSize) {
        List<UserDTO> users = dataContext.Users.ToList()
            .OrderByDescending(user => user.Id)
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .Select(user => userConverter.entityToDto(user)).ToList();

        return responseList.responseSuccess("Success", users);
    }

    public ResponseObject<UserDTO> update(UserUpdateRequest request, int id) {
        
        User user = dataContext.Users.Where(user => user.Id.Equals(id)).FirstOrDefault();
        if (user == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "User not found with id: " + id, null);
        }

        userConverter.updateToEntity(user, request);
        user.ModifiedDate = DateTime.Now;
        dataContext.SaveChanges();
        return responseObject.responseSuccess("Updated successfully", userConverter.entityToDto(user));
    }

    public ResponseObject<UserDTO> updateAvatar(IFormFile file, int id) {
        if (file == null) {
            return responseObject.responseError(StatusCodes.Status400BadRequest,
                "File is not exists", null);
        }

        if (isImageFile(file)) {
            User user = dataContext.Users.Where(user => user.Id.Equals(id)).FirstOrDefault();
            user.AvatarPhoto = fileHandler.generatePath(file, id, "avatar");
            user.ModifiedDate = DateTime.Now;
            dataContext.SaveChanges();
            
            return responseObject.responseSuccess("Update avatar successfully", userConverter.entityToDto(user));
        }
        
        return responseObject.responseError(StatusCodes.Status400BadRequest, "Error unrecognized", null);
    }

    public ResponseObject<UserDTO> updateCover(IFormFile file, int id) {
        if (file == null) {
            return responseObject.responseError(StatusCodes.Status400BadRequest,
                "File is not exists", null);
        }

        if (isImageFile(file)) {
            User user = dataContext.Users.Where(user => user.Id.Equals(id)).FirstOrDefault();
            user.CoverPhoto = fileHandler.generatePath(file, id, "cover");
            user.ModifiedDate = DateTime.Now;
            dataContext.SaveChanges();
            
            return responseObject.responseSuccess("Update cover successfully", userConverter.entityToDto(user));
        }
        return responseObject.responseError(StatusCodes.Status400BadRequest, "Error unrecognized", null);
    }

    // private string handle(IFormFile file, int id, string dir) {
    //     string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
    //     string currentDir = Directory.GetCurrentDirectory();
    //     string relativePath = Path.Combine($"\\resource\\images\\{dir}", id.ToString(), newFileName);
    //     Directory.CreateDirectory(Path.Combine($"resource/images/{dir}", id.ToString()));  //create directory if not existed
    //     string absolutePath = Path.ChangeExtension(currentDir + relativePath, "png");
    //     foreach (var f in Directory.GetFiles(currentDir + $"/resource/images/{dir}/" + id.ToString())) {
    //         File.Delete(f);
    //     }
    //     using (var fileStream = new FileStream(absolutePath, FileMode.Create)) {
    //         file.CopyTo(fileStream);
    //     }
    //
    //     return relativePath.Substring(1).Replace("\\", "%2F");  //%2F == /
    // }

    private bool isImageFile(IFormFile file) {
        string[] allowImageTypes = { "image/jpeg", "image/png" };
        if (!allowImageTypes.Contains(file.ContentType)) {
            return false;
        }

        return true;
    }

    public ResponseObject<string> assign(int adminId, int userId) {
        User user = dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefault();
        if (user == null) {
            return responseString.responseError(StatusCodes.Status404NotFound,
                "User not found with id: " + userId, null);
        }
        
        if (user.Id.Equals(adminId)) {
            return responseString.responseError(StatusCodes.Status400BadRequest,
                "Admin can't assign yourself", null);
        }

        user.Role = user.Role.Equals("USER_ROLE")
            ? Role.MOD_ROLE.ToString()
            : Role.USER_ROLE.ToString();

        dataContext.SaveChanges();
        return responseString.responseSuccess("Assign success", user.Role);
    }

    public ResponseObject<UserDTO> changePassword(int userId, string oldPassword, string newPassword) {
        User user = dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefault();

        string oldPasswordRequest = securityConfiguration.encodePassword(oldPassword, user.Salt);
        if (!oldPasswordRequest.Equals(user.Password)) {
            return responseObject.responseError(StatusCodes.Status400BadRequest,
                "Incorrect password", null);
        }

        string newPasswordEncoded = securityConfiguration.encodePassword(newPassword, user.Salt);
        user.Password = newPasswordEncoded;
        user.ModifiedDate = DateTime.Now;

        dataContext.SaveChanges();
        return responseObject.responseSuccess("Success", userConverter.entityToDto(user));
    }
}