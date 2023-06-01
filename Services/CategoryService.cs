using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;

namespace Writing.Services; 

public interface CategoryService {

    ResponseObject<CategoryDTO> create(CategoryRequest request);

    ResponseObject<CategoryDTO> update(CategoryRequest request, int id);

    ResponseObject<CategoryDTO> getById(int id);

    ResponseObject<List<CategoryDTO>> getAll(int pageNum, int pageSize);

    ResponseObject<CategoryDTO> hardDelete(int id);
}