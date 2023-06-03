using Writing.Enumerates;
using Writing.Payloads.Responses;

namespace Writing.Services
{
    public interface CommentService
    {
        Task<ResponseData<ActionStatus>> userCmtPost(int? userId, int postId, string content);
        Task<ResponseData<ActionStatus>> userSubCmtPost(int? userId, string content, int cmtId);
    }
}
