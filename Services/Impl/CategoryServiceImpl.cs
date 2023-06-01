using Writing.Controllers;
using Writing.Entities;
using Writing.Payloads.Converters;
using Writing.Payloads.DTOs;
using Writing.Payloads.Requests;
using Writing.Payloads.Responses;
using Writing.Repositories;

namespace Writing.Services.Impl; 

public class CategoryServiceImpl : CategoryService {

    private readonly DataContext dataContext;
    private readonly ResponseObject<CategoryDTO> responseObject;
    private readonly CategoryConverter categoryConverter;

    public CategoryServiceImpl(DataContext dataContext, ResponseObject<CategoryDTO> responseObject,
        CategoryConverter categoryConverter) {
        this.dataContext = dataContext;
        this.responseObject = responseObject;
        this.categoryConverter = categoryConverter;
    }
    
    public ResponseObject<CategoryDTO> create(CategoryRequest request) {
        Category category = new Category { Name = request.Name };
        dataContext.Categories.Add(category);
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Success", categoryConverter.entityToDto(category));
    }
}