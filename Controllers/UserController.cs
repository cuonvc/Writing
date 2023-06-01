using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
[Route("/api/user")]
public class UserController : Controller {

    private readonly UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    [HttpGet("/{id}")]
    public IActionResult getUser(int id) {
        ResponseObject<UserDTO> responseObject = userService.getById(id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpGet("/all")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult getAll(int pageNo, int pageSize) {
        if (!HttpContext.User.FindFirst("Role").Value.Equals("ADMIN_ROLE")) {
            return new ObjectResult("Resource access by Admin") {
                StatusCode = 403
            };
        }

        ResponseObject<List<UserDTO>> responseList = userService.getAll(pageNo, pageSize);
        return Ok(responseList);
    }

    [HttpPut("/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult update([FromBody] UserUpdateRequest request) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.update(request, id);
        
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPut("/avatar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult updateAvatarPhoto(IFormFile file) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.updateAvatar(file, id);
        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPut("/cover")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult updateCoverPhoto(IFormFile file) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.updateCover(file, id);
        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }
}