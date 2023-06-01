using Microsoft.EntityFrameworkCore;
using Writing.Entities;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;

namespace Writing.Services.Impl; 

public class UserServiceImpl : UserService {

    private readonly DataContext dataContext;
    private readonly ResponseObject<UserDTO> responseObject;
    private readonly ResponseObject<List<UserDTO>> responseList;
    private readonly UserConverter userConverter;
    private readonly IHttpContextAccessor httpContextAccessor;

    public UserServiceImpl(DataContext dataContext, ResponseObject<UserDTO> responseObject,
        ResponseObject<List<UserDTO>> responseList, UserConverter userConverter,
        IHttpContextAccessor httpContextAccessor) {
        this.dataContext = dataContext;
        this.responseObject = responseObject;
        this.responseList = responseList;
        this.userConverter = userConverter;
        this.httpContextAccessor = httpContextAccessor;
    }

    public ResponseObject<UserDTO> getById(int id) {
        User user = dataContext.Users.Where(user => user.Id.Equals(id)).FirstOrDefault();

        if (user == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "User not found with id: " + id, null);
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
            user.AvatarPhoto = handle(file, id, "avatar");
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
            user.CoverPhoto = handle(file, id, "cover");
            dataContext.SaveChanges();
            
            return responseObject.responseSuccess("Update cover successfully", userConverter.entityToDto(user));
        }
        return responseObject.responseError(StatusCodes.Status400BadRequest, "Error unrecognized", null);
    }

    private string handle(IFormFile file, int id, string dir) {
        string newFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        string currentDir = Directory.GetCurrentDirectory();
        string relativePath = Path.Combine($"\\resource\\images\\{dir}", id.ToString(), newFileName);
        Directory.CreateDirectory(Path.Combine($"resource/images/{dir}", id.ToString()));  //create directory if not existed
        string absolutePath = Path.ChangeExtension(currentDir + relativePath, "png");
        foreach (var f in Directory.GetFiles(currentDir + $"/resource/images/{dir}/" + id.ToString())) {
            File.Delete(f);
        }
        using (var fileStream = new FileStream(absolutePath, FileMode.Create)) {
            file.CopyTo(fileStream);
        }

        return relativePath.Substring(1).Replace("\\", "%2F");  //%2F == /
    }

    private bool isImageFile(IFormFile file) {
        string[] allowImageTypes = { "image/jpeg", "image/png" };
        if (!allowImageTypes.Contains(file.ContentType)) {
            return false;
        }

        return true;
    }
}