using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface PostService {

    ResponseObject<PostDTO> createPost(int userId, PostRequest postRequest, List<string> categories);
    
}