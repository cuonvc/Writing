using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Writing.Entities;
using Writing.Handle;
using Writing.Enumerates;
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
    private readonly ResponseObject<List<PostDTO>> responseList;
    private readonly ResponseObject<string> responseString;
    private readonly FileHandler fileHandler;
    private readonly IDistributedCache distributedCache;
    private readonly IHttpContextAccessor httpContextAccessor;

    public PostServiceImpl(DataContext dataContext, PostConverter postConverter,
        ResponseObject<PostDTO> responseObject, IDistributedCache distributedCache,
        FileHandler fileHandler, ResponseObject<string> responseString,
        IHttpContextAccessor httpContextAccessor, ResponseObject<List<PostDTO>> responseList) {
        this.dataContext = dataContext;
        this.postConverter = postConverter;
        this.responseObject = responseObject;
        this.distributedCache = distributedCache;
        this.fileHandler = fileHandler;
        this.responseString = responseString;
        this.httpContextAccessor = httpContextAccessor;
        this.responseList = responseList;
    }
    
    public ResponseObject<PostDTO> submitPostCreate(int userId, PostRequest postRequest, List<string> categories) {
        List<Category> categoryList = categories
            .Select(category => dataContext.Categories
                .Where(c => c.Name.Equals(category)).FirstOrDefault())
            .ToList();

        Post postPending = dataContext.Posts
            .FromSql($"SELECT * FROM Posts_tbl WHERE userId = {userId} AND isActive = 0")
            .OrderBy(post => post.Id)
            .Last();
        
        postPending.isActive = true;
        postConverter.requestToEntity(postRequest, postPending);
        postPending.User = dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefault();
        postPending.Categories = categoryList;
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Success", postConverter.entityToDto(postPending));
    }

    public ResponseObject<string> cacheThumbnail(int userId, IFormFile file) {
        //init post to get ID
        Post initPost = new Post {Title = "", Content = "", isActive = false};
        initPost.Categories = new List<Category>();
        initPost.User = dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefault();
        dataContext.Posts.Add(initPost);
        dataContext.SaveChanges();
        
        HttpContext context = httpContextAccessor.HttpContext;
        string baseUrl = $"{context.Request.Scheme}://{context.Request.Host}/image/";
        string pathFileImage = fileHandler.generatePath(file, initPost.Id, "post");
        
        initPost.Thumbnail = pathFileImage;
        dataContext.SaveChanges();
        //caching key-value = userId-pathThumb
        // distributedCache.SetString(userId.ToString(), pathFileImage);
        return responseString.responseSuccess("Success", baseUrl + pathFileImage);
    }

    public ResponseObject<PostDTO> getById(int id) {
        Post post = dataContext.Posts.Include(entity => entity.User)
            .Include(entity => entity.Categories)
            .Where(post => post.Id.Equals(id) && post.isActive == true)
            .FirstOrDefault();

        if (post == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound,
                "Post not found with id: " + id, null);
        }

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
    

    public ResponseObject<PostDTO> UpdatePost(int userId, int postId, PostRequest request, List<string> categories)
    {
        var postToUpdate = dataContext.Posts
            .Include(entity => entity.User)
            .Include(entity => entity.Categories)
            .FirstOrDefault(post => post.Id.Equals(postId));

        if (!postToUpdate.User.Id.Equals(userId)) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "Post doesn't belong to you", null);
        }
        
        if (postToUpdate == null)
        {
            return responseObject.responseError(StatusCodes.Status404NotFound, "Post not found or", null);
        }

        Post postTemporary = dataContext.Posts
            .Where(post => post.User.Id.Equals(userId) && post.isActive == false)
            .OrderBy(post => post.Id)
            .Last();  //nullable -> ok

        //if update thumbnail -> get thumbnail from temporary post and delete itself
        if (postTemporary != null) {
            postToUpdate.Thumbnail = postTemporary.Thumbnail;
            dataContext.Posts.Remove(postTemporary);
        }

        postConverter.requestToEntity(request, postToUpdate);

        var updatedCategoryList = categories
            .Select(category => dataContext.Categories.FirstOrDefault(c => c.Name.Equals(category)))
            .ToList();
        postToUpdate.Categories.Clear();
        postToUpdate.Categories.AddRange(updatedCategoryList);

        dataContext.SaveChanges();
        var updatedPostDto = postConverter.entityToDto(postToUpdate);
        return responseObject.responseSuccess("Updated successfully", updatedPostDto);
    }
    
    public ResponseObject<List<PostDTO>> GetPostsByName(string? name, int pageNumber, int pageSize)
    {
        List<PostDTO> postDTOs = dataContext.Posts
            .Include(x => x.User)
            .Include(x => x.Categories)
            .Where(x => x.Title.Trim().ToLower().Contains(name.ToLower().Trim()) && x.isActive == true)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => postConverter.entityToDto(x))
            .ToList();
        return responseList.responseSuccess("Success", postDTOs);
    }
    
    public async Task<ResponseData<ActionStatus>> PinPost(int postId)
    {
        ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();

        Post post = await dataContext.Posts.FindAsync(postId);

        if (post == null)
        {
            result.Data = ActionStatus.NOTFOUND;
            result.Status = ActionStatus.FAILED;
            result.message = $"Bài viết có id: {postId} không tồn tại";
            return result;
        }

        if (post.Pined == true) {
            post.Pined = false;
        } else {
            post.Pined = true;
        }
        
        dataContext.Posts.Update(post);
        await dataContext.SaveChangesAsync();

        result.Data = ActionStatus.SUCCESSFULLY;
        result.Status = ActionStatus.SUCCESSFULLY;
        result.message = "Bài viết đã được ghim thành công!";

        return result;
    }
    public async Task<ResponseData<ActionStatus>> userLikePost(int userId, int postId, bool vote) {
        ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();
        if (!await dataContext.Users.AnyAsync(x => x.Id == userId)) {
            result.Data = ActionStatus.NOTFOUND;
            result.Status = ActionStatus.FAILED;
            result.message = $"Người dùng có id: {userId} không tồn tại";
            return result;
        }
        
        Post post = await dataContext.Posts.FindAsync(postId);
        if (post == null) {
            result.Data = ActionStatus.NOTFOUND;
            result.Status = ActionStatus.FAILED;
            result.message = $"Bài viết có id: {userId} không tồn tại";
            return result;
        }

        if (!vote) {
            post.VoteDown += 1;
        } else {
            post.VoteUp += 1;
        }
        
        dataContext.Posts.Update(post);
        await dataContext.SaveChangesAsync();
        result.Data = ActionStatus.SUCCESSFULLY;
        result.Status = ActionStatus.SUCCESSFULLY;
        result.message = "Cập nhật lượt vote thành công";
        return result;
    }
}