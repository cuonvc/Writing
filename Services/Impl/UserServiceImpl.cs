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

    public UserServiceImpl(DataContext dataContext, ResponseObject<UserDTO> responseObject,
        ResponseObject<List<UserDTO>> responseList, UserConverter userConverter) {
        this.dataContext = dataContext;
        this.responseObject = responseObject;
        this.responseList = responseList;
        this.userConverter = userConverter;
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
}