using Writing.Entities;
using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface PostService {

    ResponseObject<PostDTO> submitPostCreate(int userId, PostRequest postRequest, List<string> categories);
    ResponseObject<string> cacheThumbnail(int userId, IFormFile file);
    ResponseObject<PostDTO> DeletePost(int postId);
    ResponseObject<PostDTO> getById(int id);
    ResponseObject<PostDTO> UpdatePost(int userId, int postId, PostRequest updatedPostRequest, List<string> updatedCategories);
    ResponseObject<List<PostDTO>> GetPostsByName(string? name, int pageNumber, int pageSize);
    ResponseObject<List<PostDTO>> getAll(int pageNum, int pageSize);
    Task<ResponseObject<ActionStatus>> PinPost(int postId);
    Task<ResponseObject<ActionStatus>> userLikePost(int userId, int postId, bool vote);
    
}