using Writing.Entities;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;

namespace Writing.Payloads.Converters; 

public class PostConverter {

    private readonly CategoryConverter categoryConverter;
    private readonly UserConverter userConverter;

    public PostConverter(CategoryConverter categoryConverter, UserConverter userConverter) {
        this.categoryConverter = categoryConverter;
        this.userConverter = userConverter;
    }

    public Post requestToEntity(PostRequest request) {
        return new Post {
            Title = request.Title,
            Content = request.Content,
            Description = request.Description,
            Thumbnail = request.Thumbnail
        };
    }

    public PostDTO entityToDto(Post entity) {
        return new PostDTO {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            Content = entity.Content,
            Thumbnail = entity.Thumbnail,
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