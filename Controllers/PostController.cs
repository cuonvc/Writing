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
public class PostController : Controller {

    private readonly PostService postService;

    public PostController(PostService postService) {
        this.postService = postService;
    }

    [HttpPost]
    [Route("/api/post")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult create([FromHeader] List<string> cateogries, PostRequest postRequest) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<PostDTO> responseObject = postService.createPost(id, postRequest, cateogries);
        return Ok(responseObject);
    }
    
    [HttpPut]
    [Route("/api/post/{id}")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult update(int id, PostRequest request, [FromHeader] List<string> categories)
    {
        ResponseObject<PostDTO> responseObject = postService.UpdatePost(id, request, categories);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }
        return Ok(responseObject);
    }

    [HttpGet]
    [Route("/api/post/{id}")]
    public IActionResult getById(int id) {
        ResponseObject<PostDTO> responseObject = postService.getById(id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpDelete]
    [Route("/api/post/{id}")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult deletePost(int id)
    {
        var role = HttpContext.User.FindFirst("Role").Value;
        
        if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
            return new ObjectResult("Resource access by Admin and Moderators") {
                StatusCode = 403
            };
        }
        
        ResponseObject<PostDTO> responseObject = postService.DeletePost(id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }
        return Ok(responseObject);
    }

    [HttpGet]
    [Route("/api/post/list")]
    public IActionResult getPosts([FromHeader] string name, [FromHeader] int pageNumber, [FromHeader] int pageSize)
    {
        List<PostDTO> postDTOs = postService.GetPostsByName(name, pageNumber, pageSize);
        return Ok(postDTOs);
    }
    [HttpGet("/api/post/userlikepost")]
    public async Task<IActionResult> userlikePost(int userId, int postId, bool userLike)
    {
        return Ok(await postService.userLikePost(userId, postId, userLike));
    }
    [HttpPut("/api/post/pin-post")]

    public async Task<IActionResult> PinPost(int postId)
    {
        return Ok(await postService.PinPost(postId));
    }

}