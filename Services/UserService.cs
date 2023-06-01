﻿using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface UserService {

    ResponseObject<UserDTO> getById(int id);

    ResponseObject<List<UserDTO>> getAll(int pageNo, int pageSize);

    ResponseObject<UserDTO> update(UserUpdateRequest request, int id);

    ResponseObject<UserDTO> updateAvatar(IFormFile file, int id);

    ResponseObject<UserDTO> updateCover(IFormFile file, int id);
}