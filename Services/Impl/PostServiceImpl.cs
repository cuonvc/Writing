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
    private readonly ResponseObject<ActionStatus> responseActionStatus;
    private readonly FileHandler fileHandler;
    private readonly IDistributedCache distributedCache;
    private readonly IHttpContextAccessor httpContextAccessor;

    public PostServiceImpl(DataContext dataContext, PostConverter postConverter,
        ResponseObject<PostDTO> responseObject, IDistributedCache distributedCache,
        FileHandler fileHandler, ResponseObject<string> responseString,
        IHttpContextAccessor httpContextAccessor, ResponseObject<List<PostDTO>> responseList,
        ResponseObject<ActionStatus> responseActionStatus) {
        this.dataContext = dataContext;
        this.postConverter = postConverter;
        this.responseObject = responseObject;
        this.distributedCache = distributedCache;
        this.fileHandler = fileHandler;
        this.responseString = responseString;
        this.httpContextAccessor = httpContextAccessor;
        this.responseList = responseList;
        this.responseActionStatus = responseActionStatus;
    }
    
    public ResponseObject<PostDTO> submitPostCreate(int userId, PostRequest postRequest, List<int> categoryIds) {
        User user = dataContext.Users.Where(user => user.Id.Equals(userId) && user.IsActive == true).FirstOrDefault();
        if (user == null) {
            return responseObject.responseError(StatusCodes.Status400BadRequest, "User chưa active tài khoản", null);
        }
        
        List<Category> categoryList = categoryIds
            .Select(categoryId => dataContext.Categories
                .Where(c => c.Id.Equals(categoryId)).FirstOrDefault())
            .ToList();

        Post postPending = dataContext.Posts
            .FromSql($"SELECT * FROM Posts_tbl WHERE userId = {userId} AND IsPending = 1")
            .OrderBy(post => post.Id)
            .LastOrDefault();

        //create post not contains image
        if (postPending == null) {
            postPending = new Post();
            postPending.Thumbnail = "resource%2Fimages%2Fdefault%2Fdefault-thumbnail.png";
            dataContext.Posts.Add(postPending);
        }
        
        postPending.IsPending = false;
        postConverter.requestToEntity(postRequest, postPending);
        postPending.User = user;
        postPending.Categories = categoryList;
        postPending.ModifiedDate = DateTime.Now;
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Success", postConverter.entityToDto(postPending));
    }

    public ResponseObject<string> cacheThumbnail(int userId, IFormFile file) {

        if (dataContext.Users.Where(user => user.Id.Equals(userId) && user.IsActive == true).FirstOrDefault() == null) {
            return responseString.responseError(StatusCodes.Status400BadRequest, "User chưa active tài khoản", null);
        }
        
        //init post to get ID
        Post initPost = new Post {Title = "", Content = "", IsPending = true};
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
            .Where(post => post.Id.Equals(id) && post.IsActive == true)
            .FirstOrDefault();
        //select * from post where id = "100"

        if (post == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound,
                "Post not found with id: " + id, null);
        }

        post.View++;
        dataContext.SaveChanges();
        return responseObject.responseSuccess("Success", postConverter.entityToDto(post));
    }

    public ResponseObject<PostDTO> DeletePost(int postId)
    {
        var postToDelete = dataContext.Posts.FirstOrDefault(x => x.Id.Equals(postId));

        if (postToDelete == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, "Không tìm thấy bài viết", null);
        }
        
//hard delete
        // //remove comments by post
        // dataContext.RemoveRange(dataContext.Comments.Where(c => c.Post.Id.Equals(postId)));
        //
        // //clear in postCategory table
        // dataContext.Posts
        //     .Include(entity => entity.Categories)
        //     .FirstOrDefault(post => post.Id.Equals(postId))
        //     .Categories.Clear();
        //
        // //clear in userPostVote table
        // dataContext.UserPostVotes.RemoveRange(dataContext.UserPostVotes
        //     .Where(uv => uv.Post.Id.Equals(postId)));
        //
        // dataContext.Posts.Remove(postToDelete);
//soft delete
        postToDelete.IsActive = false;

        dataContext.SaveChanges();
        return responseObject.responseSuccess("Xóa bài đăng thành công", new PostDTO());
    }
    

    public ResponseObject<PostDTO> UpdatePost(int userId, int postId, PostRequest request, List<int> categoryIds) {
        User user = dataContext.Users.Where(user => user.IsActive == true && user.Id.Equals(userId)).FirstOrDefault();
        if (user == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "User chưa active tài khoản", null);
        }
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
            .Where(post => post.User.Id.Equals(userId) && post.IsPending == true)
            .OrderBy(post => post.Id)
            .LastOrDefault();  //nullable -> ok

        //if update thumbnail -> get thumbnail from temporary post and delete itself
        if (postTemporary != null) {
            postToUpdate.Thumbnail = postTemporary.Thumbnail;
            dataContext.Posts.Remove(postTemporary);
        }

        postToUpdate.IsPending = false;
        postConverter.requestToEntity(request, postToUpdate);

        var updatedCategoryList = categoryIds
            .Select(id => dataContext.Categories.FirstOrDefault(c => c.Id.Equals(id)))
            .ToList();
        
        postToUpdate.Categories.Clear();
        postToUpdate.Categories = updatedCategoryList;

        postToUpdate.ModifiedDate = DateTime.Now;
        dataContext.SaveChanges();
        var updatedPostDto = postConverter.entityToDto(postToUpdate);
        return responseObject.responseSuccess("Updated successfully", updatedPostDto);
    }
    
    public ResponseObject<List<PostDTO>> GetPostsByName(string? name, int pageNumber, int pageSize)
    {
        List<PostDTO> postDTOs = dataContext.Posts
            .Include(x => x.User)
            .Include(x => x.Categories)
            .Where(x => x.Title.Trim().ToLower().Contains(name.ToLower().Trim()) && x.IsActive == true && x.IsPending == false)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .OrderByDescending(post => post.CreatedDate)
            .Select(x => postConverter.entityToDto(x))
            .ToList();
        return responseList.responseSuccess("Success", postDTOs);
    }

    public ResponseObject<List<PostDTO>> getAll(int pageNum, int pageSize) {
        List<PostDTO> postDTOs = dataContext.Posts
            .Include(x => x.User)
            .Include(x => x.Categories)
            .Where(post => post.IsActive == true && post.IsPending == false)
            .OrderByDescending(post => post.CreatedDate)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(x => postConverter.entityToDto(x))
            .ToList();
        return responseList.responseSuccess("Success", postDTOs);
    }

    public async Task<ResponseObject<ActionStatus>> PinPost(int postId)
    {
        Post post = await dataContext.Posts.FindAsync(postId);

        if (post == null)
        {
            return responseActionStatus.responseError(StatusCodes.Status404NotFound,
                $"Bài viết có id: {postId} không tồn tại", ActionStatus.NOTFOUND);
        }

        if (post.Pined == true) {
            post.Pined = false;
        } else {
            post.Pined = true;
        }
        
        dataContext.Posts.Update(post);
        await dataContext.SaveChangesAsync();

        return responseActionStatus.responseSuccess("Success", ActionStatus.SUCCESSFULLY);
    }
    
    public async Task<ResponseObject<string>> votePost(int userId, int postId, string voteType) {

        User user = await dataContext.Users.Where(user => user.Id.Equals(userId)).FirstOrDefaultAsync();

        Post post = await dataContext.Posts.FindAsync(postId);
        if (post == null) {
            return responseString.responseError(StatusCodes.Status404NotFound,
                $"Bài viết có id: {postId} không tồn tại", null);
        }
        
        UserPostVote userPostVote = dataContext.UserPostVotes
            .Where(vote => vote.Post.Equals(post) && vote.User.Equals(user))
            .FirstOrDefault();

        if (userPostVote == null) {
            dataContext.UserPostVotes.Add(new UserPostVote { User = user, Post = post, VoteType = voteType });
        } else {
            if (userPostVote.VoteType.Equals(voteType)) {
                return responseString.responseError(StatusCodes.Status400BadRequest,
                    $"This user has {userPostVote.VoteType}VOTE", null);
            }

            userPostVote.VoteType = voteType;
        }

        post.Vote += voteType.Equals(nameof(VoteType.UP)) ? 1 : -1;
        dataContext.Posts.Update(post);
        
        await dataContext.SaveChangesAsync();
        return responseString.responseSuccess("Success", voteType);
    }
}