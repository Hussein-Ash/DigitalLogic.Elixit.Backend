using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.StoreAddres;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface IStoreAddresService
{
    Task<(List<StoreAddresDto>? dtos, int? totalCount, string? error)> GetAll(StoreAddresFilter filter);

    Task<(StoreAddresDto? dto, string? error)> Add(StoreAddresForm form);

    Task<(StoreAddresDto? dto, string? error)> Update(Guid id, StoreAddresUpdate update);

    Task<(StoreAddresDto? Dto, string? error)> Delete(Guid id);

    Task<(StoreAddresDto? Dto, string? error)> GetById(Guid id);

}

public class StoreAddresService : IStoreAddresService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public StoreAddresService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(StoreAddresDto? dto, string? error)> Add(StoreAddresForm form)
    {
        var storeAddres = _mapper.Map<StoreAddres>(form);
        var result = (await _context.StoreAddress.AddAsync(storeAddres)).Entity;
        if (result == null)
            return (null, "Error while adding store address");
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreAddresDto>(result), null);

    }

    public async Task<(StoreAddresDto? Dto, string? error)> Delete(Guid id)
    {
        var storeAddres = await _context.StoreAddress.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (storeAddres == null)
            return (null, "Store address not found");
        storeAddres.Deleted = true;
        _context.StoreAddress.Update(storeAddres);
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreAddresDto>(storeAddres), null);
    }

    public async Task<(List<StoreAddresDto>? dtos, int? totalCount, string? error)> GetAll(StoreAddresFilter filter)
    {
        var query = _context.StoreAddress.AsNoTracking().Where(x => !x.Deleted);
        var totalCount = await query.CountAsync();
        var dtos = await query
            .Paginate(filter)
            .ProjectTo<StoreAddresDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(StoreAddresDto? Dto, string? error)> GetById(Guid id)
    {
        var content = await _context.StoreAddress.FirstOrDefaultAsync(x => x.Id == id);
        if (content == null) return (null, "not found");
        var contentDto = _mapper.Map<StoreAddresDto>(content);
        return (contentDto, null);
    }

    public async Task<(StoreAddresDto? dto, string? error)> Update(Guid id, StoreAddresUpdate update)
    {
      var storeAddres = await _context.StoreAddress.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (storeAddres == null)
            return (null, "Store address not found");
        _mapper.Map(update, storeAddres);
        _context.StoreAddress.Update(storeAddres);
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreAddresDto>(storeAddres), null);
    }
}
