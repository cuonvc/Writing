using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
public class AuthController : Controller {

    private readonly AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    [HttpPost]
    [Route("/api/auth/signup")]
    public async Task<IActionResult> register(RegisterRequest request) {
        ResponseObject<UserDTO> responseObject = await authService.register(request);
        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }
        
        return Ok(responseObject);
    }

    [HttpPost]
    [Route("/api/auth/login")]
    public IActionResult login(LoginRequest request) {
        ResponseObject<ResponseTokenObject> responseObject = authService.login(request);
        if (responseObject == null) {
            return Unauthorized(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPost]
    [Route("/api/auth/token/renew")]
    public IActionResult renewAccessToken(TokenObjectRequest request) {
        ResponseObject<ResponseTokenObject> responseObject = authService.renewAccessToken(request);
        if (responseObject == null) {
            return Unauthorized(responseObject);
        }

        return Ok(responseObject);
    }
}