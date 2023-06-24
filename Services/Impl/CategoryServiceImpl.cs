using Microsoft.EntityFrameworkCore;
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
    private readonly ResponseObject<List<CategoryDTO>> responseList;
    private readonly CategoryConverter categoryConverter;

    public CategoryServiceImpl(DataContext dataContext, ResponseObject<CategoryDTO> responseObject,
        CategoryConverter categoryConverter, ResponseObject<List<CategoryDTO>> responseList) {
        this.dataContext = dataContext;
        this.responseObject = responseObject;
        this.responseList = responseList;
        this.categoryConverter = categoryConverter;
    }
    
    public ResponseObject<CategoryDTO> create(CategoryRequest request) {
        Category category = dataContext.Categories.Where(category => category.Name.Equals(request.Name))
            .FirstOrDefault(); 
        if (category != null) {
            return responseObject.responseError(StatusCodes.Status400BadRequest, 
                "Category already existed", null);
        }
        
        category = new Category { Name = request.Name };
        
        dataContext.Categories.Add(category);
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Success", categoryConverter.entityToDto(category));
    }

    public ResponseObject<CategoryDTO> update(CategoryRequest request, int id) {
        Category categoryKey = dataContext.Categories.Where(category => category.Id.Equals(id)).FirstOrDefault();
        if (categoryKey == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound, 
                "Category not found with id: " + id, null);
        }
        
        Category categoryByName = dataContext.Categories.Where(category => category.Name.Equals(request.Name))
            .FirstOrDefault();
        if (categoryByName != null) {
            return responseObject.responseError(StatusCodes.Status400BadRequest, 
                "Category already existed", null);
        }

        categoryKey.Name = request.Name;
        dataContext.SaveChanges();

        return responseObject.responseSuccess("Success", categoryConverter.entityToDto(categoryKey));
    }

    public ResponseObject<CategoryDTO> getById(int id) {
        Category category = dataContext.Categories.Where(category => category.Id.Equals(id)).FirstOrDefault();
        if (category == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound,
                "Category not found with id: " + id, null);
        }

        return responseObject.responseSuccess("Success", categoryConverter.entityToDto(category));
    }

    public ResponseObject<List<CategoryDTO>> getAll(int pageNum, int pageSize) {
        List<CategoryDTO> categories = dataContext.Categories.ToList()
            .OrderBy(category => category.Name)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Select(entity => categoryConverter.entityToDto(entity))
            .ToList();

        return responseList.responseSuccess("Success", categories);
    }

    public ResponseObject<CategoryDTO> hardDelete(int id) {
        Category category = dataContext.Categories.Where(category => category.Id.Equals(id)).FirstOrDefault();
        if (category == null) {
            return responseObject.responseError(StatusCodes.Status404NotFound,
                "Category not found with id: " + id, null);
        }

        dataContext.Categories.Remove(category);
        dataContext.SaveChanges();
        return responseObject.responseSuccess("Delete successfully", new CategoryDTO());
    }
}