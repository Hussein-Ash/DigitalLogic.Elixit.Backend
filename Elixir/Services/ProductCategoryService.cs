using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.ProductCategory;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface IProductCategoryService
{
    Task<(List<ProductCategoryDto>? dtos, int? totalCount, string? error)> GetAll(ProductCategoryFilter filter);

    Task<(ProductCategoryDto? dto, string? error)> Add(ProductCategoryForm form);

    Task<(ProductCategoryDto? dto, string? error)> Update(Guid id, ProductCategoryUpdate update);

    Task<(ProductCategoryDto? Dto, string? error)> Delete(Guid id);
    Task<(ProductCategoryDto? Dto, string? error)> GetById(Guid id);


}

public class ProductCategoryService : IProductCategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ProductCategoryService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(ProductCategoryDto? dto, string? error)> Add(ProductCategoryForm form)
    {
        var productCategory = _mapper.Map<ProductCategory>(form);
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == form.ProductId && x.Deleted == false);
        if (product == null)
            return (null, "Product not found");
        var result = (await _context.ProductCategories.AddAsync(productCategory)).Entity;
        if (result == null)
            return (null, "Error while adding product category");
        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductCategoryDto>(result), null);
    }

    public async Task<(ProductCategoryDto? Dto, string? error)> Delete(Guid id)
    {
        var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id && x.Deleted != true);
        if (productCategory == null)
            return (null, "Product category not found");
        productCategory.Deleted = true;
        _context.ProductCategories.Update(productCategory);
        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductCategoryDto>(productCategory), null);
    }

    public async Task<(List<ProductCategoryDto>? dtos, int? totalCount, string? error)> GetAll(ProductCategoryFilter filter)
    {
        var query = _context.ProductCategories
         .AsNoTracking()
         .Where(x => !x.Deleted);
        var totalCount = await query.CountAsync();

        var dtos = await query
            .Paginate(filter)
            .ProjectTo<ProductCategoryDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(ProductCategoryDto? Dto, string? error)> GetById(Guid id)
    {
        var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (productCategory == null)
            return (null, "Product category not found");
        var productCategoryDto = _mapper.Map<ProductCategoryDto>(productCategory);
        return (productCategoryDto, null);

    }

    public async Task<(ProductCategoryDto? dto, string? error)> Update(Guid id, ProductCategoryUpdate update)
    {
        var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (productCategory == null)
            return (null, "Product category not found");
        _mapper.Map(update, productCategory);
        _context.ProductCategories.Update(productCategory);
        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductCategoryDto>(productCategory), null);
    }
}
