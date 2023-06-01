using Microsoft.AspNetCore.Mvc;

namespace Writing.Controllers; 

[ApiController]
public class ViewImageController : Controller {
    
    [HttpGet("/image/{path}")]
    public IActionResult Get(string path) {
        // string imagePath = "resource/images/avatar/1/8da299db-bb33-4a5d-bcbc-4a685cdd70fe.png";
        path = path.Replace("%2F", "/");

        byte[] imageData = System.IO.File.ReadAllBytes(path);

        return File(imageData, "image/png");
    }
}