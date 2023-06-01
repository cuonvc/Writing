using Writing.Entities;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;

namespace Writing.Services.Impl; 

public class PostServiceImpl : PostService {

    private readonly DataContext dataContext;
    private readonly PostConverter postConverter;
    private readonly ResponseObject<PostDTO> responseObject;

    public PostServiceImpl(DataContext dataContext, PostConverter postConverter,
        ResponseObject<PostDTO> responseObject) {
        this.dataContext = dataContext;
        this.postConverter = postConverter;
        this.responseObject = responseObject;
    }
    
    public ResponseObject<PostDTO> createPost(int userId, PostRequest postRequest, List<string> categories) {
        List<Category> categoryList = categories
            .Select(category => dataContext.Categories
                .Where(c => c.Name.Equals(category)).FirstOrDefault())
            .ToList();
        
        Post post = postConverter.requestToEntity(postRequest);
        post.User = dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefault();
        post.Categories = categoryList;
        dataContext.Posts.Add(post);
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Success", postConverter.entityToDto(post));
    }
}