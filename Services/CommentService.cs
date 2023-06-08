using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;

namespace Writing.Services
{
    public interface CommentService {
        Task<ResponseObject<CommentDTO>> create(int userId, int postId, string content);
        Task<ResponseObject<CommentDTO>> update(int userId, int id, string content);

        ResponseObject<string> remove(int id);

        ResponseObject<List<CommentDTO>> getAllByPost(int postId);
    }
}
