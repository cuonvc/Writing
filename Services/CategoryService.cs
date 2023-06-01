using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface CategoryService {

    ResponseObject<CategoryDTO> create(CategoryRequest request);
}