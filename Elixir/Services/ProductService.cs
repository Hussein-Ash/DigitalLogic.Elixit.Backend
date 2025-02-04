using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.Product;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface IProductService
{
    Task<(List<ProductDto>? dtos, int? totalCount, string? error)> GetAll(ProductFilter filter);

    Task<(ProductDto? dto, string? error)> Add(ProductForm form,Guid storeId);

    Task<(ProductDto? dto, string? error)> Update(Guid id, ProductUpdate update,Guid storeId);

    Task<(ProductDto? Dto, string? error)> Delete(Guid id,Guid storeId);

    Task<(ProductDto? Dto, string? error)> GetyById(Guid id);
}

public class ProductService : IProductService
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;
    public ProductService(IMapper mapper
    , DataContext context)
    {
        _mapper = mapper;
        _dbContext = context;

    }
    public async Task<(ProductDto? dto, string? error)> Add(ProductForm form,Guid storeId)
    {
        var store = await _dbContext.Stores.FirstOrDefaultAsync(x=>x.Id == storeId);
        if(store == null) return (null,"store not found");
        var newProduct = _mapper.Map<Product>(form);
        await _dbContext.Products.AddAsync(newProduct);
        store.Post(1,0);
        await _dbContext.SaveChangesAsync();
        var ProductDto = _mapper.Map<ProductDto>(newProduct);
        return (ProductDto, null);
    }

    public async Task<(ProductDto? Dto, string? error)> Delete(Guid id,Guid storeId)
    {   
        var storeExist = await _dbContext.Stores.FirstOrDefaultAsync(x=>x.Id == storeId);
        if (storeExist == null) return (null, "store not found");

        var product = await _dbContext.Products.Include(x=>x.Store).FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (product == null) return (null, "product not found");
        product.Deleted = true;
        product.Store.Post(0,1);
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        return (_mapper.Map<ProductDto>(product), null);
    }

    public async Task<(List<ProductDto>? dtos, int? totalCount, string? error)> GetAll(ProductFilter filter)
    {
        var query = _dbContext.Products
            .AsNoTracking()
            .Where(x => !x.Deleted);
        var totalCount = await query.CountAsync();
        var dtos = await query
        .Paginate(filter)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(ProductDto? Dto, string? error)> GetyById(Guid id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (product == null)
            return (null, "Product not found");
        var productDto = _mapper.Map<ProductDto>(product);
        return (productDto, null);
    }

    public async Task<(ProductDto? dto, string? error)> Update(Guid id, ProductUpdate update,Guid storeId)
    {
        var storeExist = await _dbContext.Stores.FirstOrDefaultAsync(x=>x.Id == storeId);
        if (storeExist == null) return (null, "store not found");
        
        var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (existingProduct == null) return (null, "Product not found");
        _mapper.Map(update, existingProduct);
        _dbContext.Products.Update(existingProduct);
        await _dbContext.SaveChangesAsync();
        var productDto = _mapper.Map<ProductDto>(existingProduct);
        return (productDto, null);
    }
}
