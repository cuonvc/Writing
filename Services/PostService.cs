using Writing.Entities;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface PostService {

    ResponseObject<PostDTO> createPost(int userId, PostRequest postRequest, List<string> categories);
    ResponseObject<PostDTO> DeletePost(int postId);
    ResponseObject<PostDTO> UpdatePost(int postId, PostRequest updatedPostRequest, List<string> updatedCategories);
    List<PostDTO> GetPostsByName(string? name, int pageNumber, int pageSize);
}