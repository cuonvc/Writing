using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;

namespace Writing.Services
{
    public interface CommentService
    {
        Task<ResponseData<ActionStatus>> userCmtPost(int? userId, int postId, string content);
        Task<ResponseData<ActionStatus>> userLikePost(int userId, int postId, bool userLike);
        Task<ResponseData<List<CommentDTO>>> GetAllCommentsByPost(int postId);
        Task<ResponseData<ActionStatus>> UpdateComment(int postId, int commentId, string content);
        Task<ResponseData<ActionStatus>> DeleteComment(int postId, int commentId);
    }
}
