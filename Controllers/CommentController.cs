using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Writing.Services;

namespace Writing.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly CommentService _commentService;
        public CommentController(CommentService commentService)
        {
            _commentService = commentService;
        }
        
        
        [HttpPost("usercmtpost")]
        public async Task<IActionResult> create(int userId, int postId, string content)
        {
            return Ok(await _commentService.userCmtPost(userId, postId, content));
        }
        [HttpPut("usersubcmtpost")]
        public async Task<IActionResult> userSubCmtPost(int userId, string content, int cmtId)
        {
            return Ok(await _commentService.userSubCmtPost(userId, content, cmtId));
        }
    }
}
