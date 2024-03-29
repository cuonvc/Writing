﻿using Writing.Entities;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface AuthService {
    Task<ResponseObject<UserDTO>> register(RegisterRequest request);

    ResponseObject<ResponseTokenObject> login(LoginRequest request);

    ResponseObject<ResponseTokenObject> renewAccessToken(TokenObjectRequest request);

    string generateAccessToken(User user);
}