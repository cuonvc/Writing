using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writing.Entities;
using Writing.Enumerates;
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
    [Route("/api/post/pre-thumb")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult preUploadThumbnail(IFormFile file) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<string> responseUrl = postService.cacheThumbnail(id, file);
        return Ok(responseUrl);
    }

    [HttpPost]
    [Route("/api/post/submit")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult submitPost([FromHeader] List<string> cateogries, PostRequest postRequest) {
        int id = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<PostDTO> responseObject = postService.submitPostCreate(id, postRequest, cateogries);
        return Ok(responseObject);
    }

    [HttpPut]
    [Route("/api/post/{id}")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult update(int id, PostRequest request, [FromHeader] List<string> categories)
    {
        int userId = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<PostDTO> responseObject = postService.UpdatePost(userId, id, request, categories);
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
        ResponseObject<List<PostDTO>> postDTOs = postService.GetPostsByName(name, pageNumber, pageSize);
        return Ok(postDTOs);
    }
    
    [HttpGet]
    [Route("/api/post/all")]
    public IActionResult getAll([FromHeader] int pageNumber, [FromHeader] int pageSize)
    {
        ResponseObject<List<PostDTO>> postDTOs = postService.getAll(pageNumber, pageSize);
        return Ok(postDTOs);
    }
    
    
    [HttpGet("/api/post/vote")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> userlikePost(int postId, bool vote)
    {
        int userId = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
        ResponseObject<ActionStatus> responseObject = await postService.userLikePost(userId, postId, vote);
        if (responseObject.Data.Equals(ActionStatus.NOTFOUND)) {
            return NotFound(responseObject);
        }
        
        return Ok(responseObject);
    }
    
    
    [HttpPut("/api/post/pin")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> PinPost(int postId)
    {
        var role = HttpContext.User.FindFirst("Role").Value;
        
        if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
            return new ObjectResult("Resource access by Admin and Moderators") {
                StatusCode = 403
            };
        }

        ResponseObject<ActionStatus> responseData = await postService.PinPost(postId);
        if (responseData.Data.Equals(ActionStatus.NOTFOUND)) {
            return NotFound(responseData);
        }

        return Ok(responseData);
    }

}