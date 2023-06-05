using Writing.Entities;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;

namespace Writing.Payloads.Converters; 

public class PostConverter {

    private readonly CategoryConverter categoryConverter;
    private readonly UserConverter userConverter;
    private readonly IHttpContextAccessor httpContextAccessor;

    public PostConverter(CategoryConverter categoryConverter, UserConverter userConverter,
        IHttpContextAccessor httpContextAccessor) {
        this.categoryConverter = categoryConverter;
        this.userConverter = userConverter;
        this.httpContextAccessor = httpContextAccessor;
    }

    public Post requestToEntity(PostRequest request) {
        return new Post {
            Title = request.Title,
            Content = request.Content,
            Description = request.Description,
        };
    }

    public void requestToEntity(PostRequest request, Post entity) {
        entity.Title = request.Title;
        entity.Content = request.Content;
        entity.Description = request.Description;
    }

    public PostDTO entityToDto(Post entity) {
        HttpContext context = httpContextAccessor.HttpContext;
        string baseUrl = $"{context.Request.Scheme}://{context.Request.Host}/image/";
        return new PostDTO {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Content = entity.Content,
            Thumbnail = baseUrl + entity.Thumbnail,
            VoteUp = entity.VoteUp,
            VoteDown = entity.VoteDown,
            View = entity.View,
            Pined = entity.Pined,
            CreatedDate = entity.CreatedDate,
            CreatedBy = entity.CreatedBy,
            ModifiedDate = entity.ModifiedDate,
            ModifiedBy = entity.ModifiedBy,
            isActive = entity.isActive,
            Categories = entity.Categories.Select(category => categoryConverter.entityToDto(category)).ToList(),
            User = userConverter.entityToDto(entity.User)
        };
    }
}