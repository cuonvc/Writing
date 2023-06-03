using Microsoft.EntityFrameworkCore;
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
    public ResponseObject<PostDTO> DeletePost(int postId)
    {
        var postToDelete = dataContext.Posts.Include(x => x.User).Include(x => x.Categories).FirstOrDefault(x => x.Id == postId);

        if (postToDelete == null)
        {
            return responseObject.responseError(StatusCodes.Status404NotFound, "Không tìm thấy bài viết", null);
        }

        dataContext.Posts.Remove(postToDelete);
        dataContext.SaveChanges();

        var deletedPostDto = postConverter.entityToDto(postToDelete);
        return responseObject.responseSuccess("Xóa bài đăng thành công", deletedPostDto);
    }



    public ResponseObject<PostDTO> UpdatePost(int postId, PostRequest updatedPostRequest, List<string> updatedCategories)
    {
        var postToUpdate = dataContext.Posts
            .Include(x => x.User)
            .Include(x => x.Categories)
            .FirstOrDefault(x => x.Id == postId);

        if (postToUpdate == null)
        {
            return responseObject.responseError(StatusCodes.Status404NotFound, "Không tìm thấy bài đăng", null);
        }

        // Cập nhật thuộc tính Post
        postToUpdate.Title = updatedPostRequest.Title;
        postToUpdate.Content = updatedPostRequest.Content;
        postToUpdate.Description = updatedPostRequest.Description;
        postToUpdate.Thumbnail = updatedPostRequest.Thumbnail;

        // Cập nhật danh mục
        var updatedCategoryList = updatedCategories
            .Select(category => dataContext.Categories.FirstOrDefault(c => c.Name.Equals(category)))
            .ToList();
        postToUpdate.Categories.Clear();
        postToUpdate.Categories.AddRange(updatedCategoryList);

        dataContext.SaveChanges();

        var updatedPostDto = postConverter.entityToDto(postToUpdate);
        return responseObject.responseSuccess("Cập nhật thông tin bài đăng thành công", updatedPostDto);
    }
    public List<PostDTO> GetPostsByName(string? name, int pageNumber, int pageSize)
    {
        List<PostDTO> postDTOs = dataContext.Posts
            .Include(x => x.User)
            .Include(x => x.Categories)
            .Where(x => x.Title.Trim().ToLower().Contains(name.ToLower().Trim()))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => postConverter.entityToDto(x))
            .ToList();
        return postDTOs;
    }
}