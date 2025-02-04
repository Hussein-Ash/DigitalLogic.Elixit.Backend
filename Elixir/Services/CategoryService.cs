using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.CategoryDto;
using Elixir.Entities;
using Elixir.Helpers;
using Elixir.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface ICategoryService
{
    Task<(CategoryDto? categoryDto, string? error)> CreateCategory(CategoryForm categoryForm);
    Task<(CategoryDto? categoryDto, string? error)> CreateSubCategory(SubCategoryForm form, Guid CategoryId);

    Task<(Category? category, string? error)> DeleteCategory(Guid id);
    Task<(CategoryDto? categoryDto, string? error)> UpdateCategory(CategoryUpdate update, Guid CategoryId);
    Task<(CategoryDto? categoryDto, string? error)> GetCategoryById(Guid id);
    Task<(List<CategoryDto>? categoryDtos, int? totalCount, string? error)> GetAllCategories(CategoryFilter filter);
    Task<(List<CategoryViewsDto>? categoryDtos, int? totalCount, string? error)> GetAllCategoriesViews(CategoryFilter filter);



}

public class CategoryService : ICategoryService
{
    private readonly DataContext _dbContext;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IUserClaimsService _claims;
    private readonly IMediator _mediator;
    public CategoryService(IRepositoryWrapper repositoryWrapper, IMapper mapper
    , DataContext context, IUserClaimsService claims, IMediator mediator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _dbContext = context;
        _claims = claims;
        _mediator = mediator;
    }

    public async Task<(CategoryDto? categoryDto, string? error)> CreateCategory(CategoryForm categoryForm)
    {
        var newCategory = new Category
        {
            Name = categoryForm.Name,
            Img = categoryForm.Img,
        };
        await _dbContext.Categories.AddAsync(newCategory);
        // _mediator.Publish(new OrderPlacedNotification( new Notifications(),"All"));

        await _dbContext.SaveChangesAsync();
        var categoryDto = _mapper.Map<CategoryDto>(newCategory);
        return (categoryDto, null);


    }

    public async Task<(CategoryDto? categoryDto, string? error)> CreateSubCategory(SubCategoryForm form, Guid CategoryId)
    {
        var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == CategoryId);
        if (existingCategory == null) return (null, "Category not found");

        if (form.SubCategory != null)
        {
            var subCategoryList = new List<Category>();
            foreach (var category in form.SubCategory)
            {
                var newSubCategory = new Category
                {
                    Name = category.Name,
                    Img = category.Img,
                    ParentId = existingCategory.Id
                };
                subCategoryList.Add(newSubCategory);

            }
            existingCategory.SubCategory.AddRange(subCategoryList);
        }
        await _dbContext.SaveChangesAsync();
        var categoryDto = _mapper.Map<CategoryDto>(existingCategory);
        return (categoryDto, null);

    }

    public async Task<(Category? category, string? error)> DeleteCategory(Guid id)
    {
        var category = await _repositoryWrapper.Category.GetById(id);
        if (category == null) return (null, "already deleted");
        var deletecategory = await _repositoryWrapper.Category.SoftDelete(id);
        return (deletecategory, null);

    }

    public async Task<(List<CategoryDto>? categoryDtos, int? totalCount, string? error)> GetAllCategories(CategoryFilter filter)
    {

        var query = _dbContext.Categories
         .AsNoTracking()
         .Include(x => x.SubCategory)
         .Where(x => !x.Deleted && x.ParentId == null);
        var totalCount = await query.CountAsync();

        var dtos = await query
            .Paginate(filter)
            .ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        dtos.ForEach(async f =>
        {
            f.SubCategory = await _dbContext.Categories.Where(w => w.ParentId == f.Id).ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).ToListAsync();
        });

        return (dtos, totalCount, null);
    }

    public async Task<(List<CategoryViewsDto>? categoryDtos, int? totalCount, string? error)> GetAllCategoriesViews(CategoryFilter filter)
    {
        var query = _dbContext.Categories
         .AsNoTracking()
         .Where(x => !x.Deleted && x.ParentId != null)
         .Include(x => x.SubCategory);

        var totalCount = await query.CountAsync();

        var dtos = await query
            .Paginate(filter)
            .Select( x=> new CategoryViewsDto(){
                Category = x.ParentCategory.Name,
                SubCategory = x.Name,
                IsActive = x.IsActive,
                Videos = _dbContext.ProductCategories.Where(z=>z.CategoryId == x.Id).LongCount()
                
            })              
            .ToListAsync();


        return (dtos, totalCount, null);

    }

    public async Task<(CategoryDto? categoryDto, string? error)> GetCategoryById(Guid id)
    {
        var category = await _dbContext.Categories.Include(x => x.SubCategory).ThenInclude(x => x.SubCategory).FirstOrDefaultAsync(x => x.Id == id);
        if (category == null) return (null, "not found");
        var categoryDto = _mapper.Map<CategoryDto>(category);
        return (categoryDto, null);
    }

    public async Task<(CategoryDto? categoryDto, string? error)> UpdateCategory(CategoryUpdate update, Guid CategoryId)
    {
        var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == CategoryId);
        if (existingCategory == null) return (null, "Category not found");
        _mapper.Map(update, existingCategory);
        _dbContext.Categories.Update(existingCategory);
        await _dbContext.SaveChangesAsync();
        var categoryDto = _mapper.Map<CategoryDto>(existingCategory);
        return (categoryDto, null);
    }
}
