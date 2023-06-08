using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;
using Writing.Services;

namespace Writing.Controllers
{
    [ApiController]
    public class CommentController : Controller {
        private readonly CommentService commentService;
        public CommentController(CommentService commentService) {
            this.commentService = commentService;
        }
        
        
        [HttpPost]
        [Route("/api/comment")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> comment(int postId, [FromBody] string content) {
            int userId = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
            ResponseObject<CommentDTO> responseObject = await commentService.create(userId, postId, content);
            if (responseObject.Data == null) {
                return NotFound(responseObject);
            }
            return Ok(responseObject);
        }
        
        
        [HttpPut]
        [Route("/api/comment/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> updateComment(int id, [FromBody] string content) {
            int userId = Convert.ToInt32(HttpContext.User.FindFirst("Id").Value);
            ResponseObject<CommentDTO> responseObject = await commentService.update(userId, id, content);
            if (responseObject.Data == null) {
                return BadRequest(responseObject);
            }
            return Ok(responseObject);
        }
        
        [HttpDelete]
        [Route("/api/comment/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult delete(int id) {
            var role = HttpContext.User.FindFirst("Role").Value;
        
            if (!role.Equals("ADMIN_ROLE") && !role.Equals("MOD_ROLE")) {
                return new ObjectResult("Resource access by Admin and Moderators") {
                    StatusCode = 403
                };
            }
            
            ResponseObject<string> responseObject = commentService.remove(id);
            if (responseObject.Data == null) {
                return BadRequest(responseObject);
            }
            return Ok(responseObject);
        }

        
        [HttpGet]
        [Route("/api/comment")]
        // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult getAllByPost(int postId) {
            return Ok(commentService.getAllByPost(postId));
        }
    }
}
