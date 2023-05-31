using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
[Route("/api/auth")]
public class AuthController : Controller {

    private readonly AuthService authService;

    public AuthController(AuthService authService) {
        this.authService = authService;
    }

    [HttpPost("/signup")]
    public IActionResult register(RegisterRequest request) {
        ResponseObject<UserDTO> responseObject = authService.register(request);
        return Ok(responseObject);
    }
}