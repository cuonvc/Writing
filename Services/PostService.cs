using Writing.Entities;
using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface PostService {

    ResponseObject<PostDTO> submitPostCreate(int userId, PostRequest postRequest, List<int> categoriIds);
    ResponseObject<string> cacheThumbnail(int userId, IFormFile file);
    ResponseObject<PostDTO> DeletePost(int postId);
    ResponseObject<PostDTO> getById(int id);
    ResponseObject<PostDTO> UpdatePost(int userId, int postId, PostRequest postRequest, List<int> categoryIds);
    ResponseObject<List<PostDTO>> GetPostsByName(string? name, int pageNumber, int pageSize);
    ResponseObject<List<PostDTO>> getAll(int pageNum, int pageSize);
    Task<ResponseObject<ActionStatus>> PinPost(int postId);
    Task<ResponseObject<string>> votePost(int userId, int postId, string voteType);
    
}