using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
public class CategoryController : Controller {

    private readonly CategoryService categoryService;

    public CategoryController(CategoryService categoryService) {
        this.categoryService = categoryService;
    }

    [HttpPost]
    [Route("/api/category")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult create(CategoryRequest request) {
        var role = HttpContext.User.FindFirst("Role").Value;
        if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
            return new ObjectResult("Resource access by Admin and Moderators") {
                StatusCode = 403
            };
        }
        
        ResponseObject<CategoryDTO> responseObject = categoryService.create(request);
        if (responseObject.Data == null) {
            return BadRequest(responseObject);
        }
        return Ok(responseObject);
    }

    [HttpPut]
    [Route("/api/category/{id}")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult update(CategoryRequest request, int id) {
        var role = HttpContext.User.FindFirst("Role").Value;
        if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
            return new ObjectResult("Resource access by Admin and Moderators") {
                StatusCode = 403
            };
        }
        
        ResponseObject<CategoryDTO> responseObject = categoryService.update(request, id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

    [HttpGet]
    [Route("/api/category/{id}")]
    public IActionResult get(int id) {
        ResponseObject<CategoryDTO> responseObject = categoryService.getById(id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }
    
        return Ok(responseObject);
    }

    [HttpGet]
    [Route("/api/category/all")]
    public IActionResult getAll(int pageNum, int pageSize) {
        ResponseObject<List<CategoryDTO>> responseObject = categoryService.getAll(pageNum, pageSize);
        return Ok(responseObject);
    }

    [HttpDelete]
    [Route("/api/category/{id}")]
    [Authorize (AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult hardDelete(int id) {
        var role = HttpContext.User.FindFirst("Role").Value;
        if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
            return new ObjectResult("Resource access by Admin and Moderators") {
                StatusCode = 403
            };
        }
        
        ResponseObject<CategoryDTO> responseObject = categoryService.hardDelete(id);
        if (responseObject.Data == null) {
            return NotFound(responseObject);
        }

        return Ok(responseObject);
    }

}