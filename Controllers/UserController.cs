using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
public class UserController : Controller {

    private readonly UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    [HttpGet]
    [Route("/api/user/{id}")]
    public IActionResult getUser(int id) {
        ResponseObject<UserDTO> responseObject = userService.getById(id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpGet]
    [Route("/api/user/all")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult getAll(int pageNo, int pageSize) {
        var role = HttpContext.User.FindFirst("Role").Value;
        if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
            return new ObjectResult("Resource access by Admin and Moderators") {
                StatusCode = 403
            };
        }

        ResponseObject<List<UserDTO>> responseList = userService.getAll(pageNo, pageSize);
        return Ok(responseList);
    }

    [HttpPut]
    [Route("/api/user/update")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult update([FromBody] UserUpdateRequest request) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.update(request, id);
        
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPut]
    [Route("/api/user/avatar")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult updateAvatarPhoto(IFormFile file) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.updateAvatar(file, id);
        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPut]
    [Route("/api/user/cover")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult updateCoverPhoto(IFormFile file) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.updateCover(file, id);
        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPut]
    [Route("/api/user/assign/{userId}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult assignRoles(int userId) {
        if (!HttpContext.User.FindFirst("Role").Value.Equals("ADMIN_ROLE")) {
            return new ObjectResult("Resource access by Admin") {
                StatusCode = 403
            };
        }
        
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<string> responseObject = userService.assign(id, userId);

        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpPut]
    [Route("/api/user/change_password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult changePassword(ChangePasswordRequest request) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<UserDTO> responseObject = userService.changePassword(id, request.oldPassword, request.newPassword);

        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }

        return Ok(responseObject);
    }
    
}