using Writing.Entities;
using Writing.Payloads.DTOs;

namespace Writing.Payloads.Converters; 

public class CommentConverter {

    private readonly UserConverter userConverter;

    public CommentConverter(UserConverter userConverter) {
        this.userConverter = userConverter;
    }

    public CommentDTO entityToDto(Comment entity) {
        return new CommentDTO {
            Id = entity.Id,
            Content = entity.content,
            User = userConverter.entityToDto(entity.User),
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate
        };
    }
}