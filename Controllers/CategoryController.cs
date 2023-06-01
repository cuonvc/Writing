using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers; 

[ApiController]
[Route("/api/category")]
public class CategoryController : Controller {

    private readonly CategoryService categoryService;

    public CategoryController(CategoryService categoryService) {
        this.categoryService = categoryService;
    }

    [HttpPost]
    public IActionResult create(CategoryRequest request) {
        ResponseObject<CategoryDTO> responseObject = categoryService.create(request);
        return Ok(responseObject);
    }
}