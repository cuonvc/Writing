using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writing.Entities;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
[Route("/api/post")]
public class PostController : Controller {

    private readonly PostService postService;

    public PostController(PostService postService) {
        this.postService = postService;
    }

    [HttpPost]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult create([FromHeader] List<string> cateogries, PostRequest postRequest) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<PostDTO> responseObject = postService.createPost(id, postRequest, cateogries);
        return Ok(responseObject);
    }

}