using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs;
using Elixir.DATA.DTOs.StoreCategory;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface IStoreCategoryService
{
    Task<(List<StoreCategoryDto>? dtos, int? totalCount, string? error)> GetAll(BaseFilter filter);

    Task<(StoreCategoryDto? dto, string? error)> Add(StoreCategoryForm form);

    Task<(StoreCategoryDto? dto, string? error)> Update(Guid id, StoreCategoryForm update);

    Task<(StoreCategoryDto? Dto, string? error)> Delete(Guid id);

    Task<(StoreCategoryDto? Dto, string? error)> GetById(Guid id);

}


public class StoreCategoryService : IStoreCategoryService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public StoreCategoryService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(StoreCategoryDto? dto, string? error)> Add(StoreCategoryForm form)
    {
        var storeCategory = _mapper.Map<StoreCategory>(form);
        var result = (await _context.StoreCategories.AddAsync(storeCategory)).Entity;
        if (result == null)
            return (null, "Error while adding store category");
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreCategoryDto>(result), null);

    }

    public async Task<(StoreCategoryDto? Dto, string? error)> Delete(Guid id)
    {
        var storeCategory = await _context.StoreCategories.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (storeCategory == null)
            return (null, "Store category not found");
        storeCategory.Deleted = true;
        _context.StoreCategories.Update(storeCategory);
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreCategoryDto>(storeCategory), null);
    }

    public async Task<(List<StoreCategoryDto>? dtos, int? totalCount, string? error)> GetAll(BaseFilter filter)
    {
        var query = _context.StoreCategories
            .AsNoTracking()
            .Where(x => !x.Deleted);

        var totalCount = await query.CountAsync();
        var dtos = await query
        .Paginate(filter)
        .ProjectTo<StoreCategoryDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(StoreCategoryDto? Dto, string? error)> GetById(Guid id)
    {
        var content = await _context.StoreCategories.FirstOrDefaultAsync(x => x.Id == id);
        if (content == null) return (null, "not found");
        var contentDto = _mapper.Map<StoreCategoryDto>(content);
        return (contentDto, null);
    }

    public async Task<(StoreCategoryDto? dto, string? error)> Update(Guid id, StoreCategoryForm update)
    {
        var storeCategory = await _context.StoreCategories.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (storeCategory == null)
            return (null, "Store category not found");
        _mapper.Map(update, storeCategory);
        _context.StoreCategories.Update(storeCategory);
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreCategoryDto>(storeCategory), null);
    }
}
