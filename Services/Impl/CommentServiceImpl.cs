using Microsoft.EntityFrameworkCore;
using Writing.Entities;
using Writing.Enumerates;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Responses;
using Writing.Repositories;

namespace Writing.Services.Impl
{
    public class CommentServiceImpl : CommentService {
        
        private readonly DataContext dataContext;
        private readonly ResponseObject<CommentDTO> responseObject;
        private readonly CommentConverter commentConverter;
        private readonly ResponseObject<List<CommentDTO>> responseList;
        private readonly ResponseObject<string> responseString;

        public CommentServiceImpl(DataContext dataContext, ResponseObject<CommentDTO> responseObject,
            CommentConverter commentConverter, ResponseObject<List<CommentDTO>> responseList,
            ResponseObject<string> responseString) {
            this.dataContext = dataContext;
            this.responseObject = responseObject;
            this.commentConverter = commentConverter;
            this.responseList = responseList;
            this.responseString = responseString;
        }
        
        public async Task<ResponseObject<CommentDTO>> create(int userId, int postId, string content) {

            Post post = await dataContext.Posts.FindAsync(postId);

            if (post == null) {
                return responseObject.responseError(StatusCodes.Status404NotFound, 
                    $"Post not found with id: {postId}", null);
            }
            
            Comment comment = new Comment {
                User = await dataContext.Users.FindAsync(userId),
                Post = post,
                content = content,
            };

            await dataContext.Comments.AddAsync(comment);
            await dataContext.SaveChangesAsync();
            return responseObject.responseSuccess("Success", commentConverter.entityToDto(comment));
        }

        public async Task<ResponseObject<CommentDTO>> update(int userId, int id, string content) {

            Comment comment = await dataContext.Comments
                .Include(comment => comment.User)
                .Where(comment => comment.Id.Equals(id) && comment.User.Id.Equals(userId))
                .FirstOrDefaultAsync();
            
            if (comment == null) {
                return responseObject.responseError(StatusCodes.Status400BadRequest,
                    "User can't update this comment", null);
            }

            comment.content = content;
            comment.ModifiedDate = DateTime.Now;
            await dataContext.SaveChangesAsync();

            return responseObject.responseSuccess("Success", commentConverter.entityToDto(comment));
        }

        public ResponseObject<string> remove(int id) {
            Comment comment = dataContext.Comments.Find(id);
            if (comment == null) {
                return responseString.responseError(StatusCodes.Status404NotFound, 
                    "Comment not found", null);
            }

            comment.IsActive = false;
            dataContext.SaveChanges();
            return responseString.responseSuccess("Success", "Remove comment successfully");
        }

        public ResponseObject<List<CommentDTO>> getAllByPost(int postId) {

            List<CommentDTO> commentDtos = dataContext.Comments
                .Include(comment => comment.User)
                .Where(comment => comment.Post.Id.Equals(postId) && comment.IsActive.Equals(true))
                .Select(comment => commentConverter.entityToDto(comment))
                .ToList();

            return responseList.responseSuccess("Success", commentDtos);
        }
    }
}
