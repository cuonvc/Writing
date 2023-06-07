using Microsoft.EntityFrameworkCore;
using Writing.Entities;
using Writing.Enumerates;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;
using Writing.Repositories;

namespace Writing.Services.Impl
{
    public class CommentServiceImpl : CommentService
    {
        
        private readonly DataContext dataContext;

        public CommentServiceImpl(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<ResponseData<ActionStatus>> userCmtPost(int? userId, int postId, string content)
        {
            ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();

            if (userId == null || !await dataContext.Users.AnyAsync(x => x.Id == userId))
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"User có id: {userId} không tồn tại";
                return result;
            }

            Post post = await dataContext.Posts.FindAsync(postId);

            if (post == null)
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Post có id: {postId} không tồn tại";
                return result;
            }

            Comment comment = new Comment();
            if (await dataContext.Users.AnyAsync(x => x.Id == userId))
            {
                comment.User = await dataContext.Users.FindAsync(userId);
            }
            else
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"User có id: {userId} không tồn tại";
                return result;
            }

            comment.Post = post;
            comment.content = content;
            comment.CreatedDate = DateTime.Now;
            comment.ModifiedDate = DateTime.Now;
            comment.isActive = false;


            await dataContext.Comments.AddAsync(comment);
            await dataContext.SaveChangesAsync();

            result.Data = ActionStatus.SUCCESSFULLY;
            result.Status = ActionStatus.SUCCESSFULLY;
            result.message = "Lấy dữ liệu thành công!";

            return result;
        }

        public async Task<ResponseData<ActionStatus>> userLikePost(int userId, int postId, bool userLike)
        {
            ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();
            if (!await dataContext.Users.AnyAsync(x => x.Id == userId))
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Người dùng có id: {userId} không tồn tại";
                return result;
            }
            Post post = await dataContext.Posts.FindAsync(postId);
            if (post == null)
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Bài viết có id: {userId} không tồn tại";
                return result;
            }
            else
            {
                if (userLike)
                {
                    if (post.VoteDown > 0)
                    {
                        post.VoteUp++;
                        post.VoteDown--;
                    }
                    else
                    {
                        post.VoteUp++;
                        post.VoteDown = 0;
                    }
                    dataContext.Posts.Update(post);
                    await dataContext.SaveChangesAsync();
                    result.Data = ActionStatus.SUCCESSFULLY;
                    result.Status = ActionStatus.SUCCESSFULLY;
                    result.message = "Cập nhật lượt like thành công";
                    return result;
                }
                else
                {
                    if (post.VoteUp > 0)
                    {
                        post.VoteDown++;
                        post.VoteUp--;
                    }
                    else
                    {
                        post.VoteDown++;
                        post.VoteUp = 0;
                    }
                    dataContext.Posts.Update(post);
                    await dataContext.SaveChangesAsync();
                    result.Data = ActionStatus.SUCCESSFULLY;
                    result.Status = ActionStatus.SUCCESSFULLY;
                    result.message = "Cập nhật lượt dislike thành công";
                    return result;
                }
            }
        }


        public async Task<ResponseData<List<CommentDTO>>> GetAllCommentsByPost(int postId)
        {
            ResponseData<List<CommentDTO>> result = new ResponseData<List<CommentDTO>>();

            if (!await dataContext.Posts.AnyAsync(x => x.Id == postId))
            {
                result.Data = null;
                result.Status = ActionStatus.FAILED;
                result.message = $"Post có id: {postId} không tồn tại";
                return result;
            }

            List<Comment> comments = await dataContext.Comments
                .Where(x => x.PostId == postId && x.isActive)
                .OrderByDescending(x => x.ModifiedDate)
.ToListAsync();

            result.Data = null;
            result.Status = ActionStatus.SUCCESSFULLY;
            result.message = "Lấy danh sách comment thành công!";

            return result;
        }

        public async Task<ResponseData<ActionStatus>> UpdateComment(int postId, int commentId, string content)
        {
            ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();

            if (!await dataContext.Posts.AnyAsync(x => x.Id == postId))
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Post có id: {postId} không tồn tại";
                return result;
            }

            Comment comment = await dataContext.Comments.FindAsync(commentId);

            if (comment == null || comment.PostId != postId)
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Comment có id: {commentId} không tồn tại trong post có id: {postId}";
                return result;
            }

            comment.content = content;
            comment.ModifiedDate = DateTime.Now;

            await dataContext.SaveChangesAsync();

            result.Data = ActionStatus.SUCCESSFULLY;
            result.Status = ActionStatus.SUCCESSFULLY;
            result.message = "Cập nhật comment thành công!";

            return result;
        }

        public async Task<ResponseData<ActionStatus>> DeleteComment(int postId, int commentId)
        {
            ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();

            if (!await dataContext.Posts.AnyAsync(x => x.Id == postId))
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Post có id: {postId} không tồn tại";
                return result;
            }

            Comment comment = await dataContext.Comments.FindAsync(commentId);

            if (comment == null || comment.PostId != postId)
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Comment có id: {commentId} không tồn tại trong post có id: {postId}";
                return result;
            }

            comment.isActive = false;
            await dataContext.SaveChangesAsync();

            result.Data = ActionStatus.SUCCESSFULLY;
            result.Status = ActionStatus.SUCCESSFULLY;
            result.message = "Xóa comment thành công!";

            return result;
        }

    }
}
