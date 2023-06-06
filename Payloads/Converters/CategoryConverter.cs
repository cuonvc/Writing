using Writing.Entities;
using Writing.Payloads.DTOs;

namespace Writing.Payloads.Converters; 

public class CategoryConverter {

    public CategoryDTO entityToDto(Category entity) {
        return new CategoryDTO {
            Name = entity.Name,
            CreatedDate = entity.CreatedDate,
            CreatedBy = entity.CreatedBy,
            ModifiedDate = entity.ModifiedDate,
            ModifiedBy = entity.ModifiedBy,
            isActive = entity.IsActive
        };
    }
}