using Microsoft.EntityFrameworkCore;
using Writing.Entities;
using Writing.Enumerates;
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
            comment.IsActive = false;

            await dataContext.Comments.AddAsync(comment);
            await dataContext.SaveChangesAsync();

            result.Data = ActionStatus.SUCCESSFULLY;
            result.Status = ActionStatus.SUCCESSFULLY;
            result.message = "Lấy dữ liệu thành công!";

            return result;
        }

        public async Task<ResponseData<ActionStatus>> userSubCmtPost(int? userId, string content, int cmtId)
        {
            ResponseData<ActionStatus> result = new ResponseData<ActionStatus>();

            User user = await dataContext.Users.FindAsync(userId);
            if (user == null)
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"User có id: {userId} không tồn tại";
                return result;
            }

            Comment cmt = await dataContext.Comments.FindAsync(cmtId);
            if (cmt == null)
            {
                result.Data = ActionStatus.NOTFOUND;
                result.Status = ActionStatus.FAILED;
                result.message = $"Comment có id {cmtId} không tồn tại";
                return result;
            }

            cmt.User = user;
            cmt.content = content;
            cmt.CreatedDate = DateTime.Now;
            cmt.ModifiedDate = DateTime.Now;
            cmt.IsActive = false;

            await dataContext.SaveChangesAsync();

            result.Data = ActionStatus.SUCCESSFULLY;
            result.Status = ActionStatus.SUCCESSFULLY;
            result.message = "Lấy dữ liệu thành công!";

            return result;
        }
    }
}
