using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Writing.Enumerates;
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
        [HttpPost]
        [Route("usercmtpost")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> userCmtPost(int postId, string content)
        {
            var userIdClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return BadRequest("Không tìm thấy thông tin người dùng.");
            }

            var result = await _commentService.userCmtPost(userId, postId, content);
            if (result.Status == ActionStatus.FAILED)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("userlikepost")]
        public async Task<IActionResult> userlikePost(int userId, int postId, bool userLike)
        {
            return Ok(await _commentService.userLikePost(userId, postId, userLike));
        }

        [HttpGet("get-all-comment")]
        public async Task<IActionResult> getAllCommentOfPost(int postId)
        {
            return Ok(await _commentService.GetAllCommentsByPost(postId));
        }
        [HttpPut("update-comment")]
        public async Task<IActionResult> updateComment(int postId, int commentId, string content)
        {
            return Ok(await _commentService.UpdateComment(postId, commentId, content));
        }
        [HttpDelete("delete-comment")]
        public async Task<IActionResult> deleteComment(int postId, int commentId)
        {
            return Ok(await _commentService.DeleteComment(postId, commentId));
        }
    }
}
