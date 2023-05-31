using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface AuthService {
    ResponseObject<UserDTO> register(RegisterRequest request);
}