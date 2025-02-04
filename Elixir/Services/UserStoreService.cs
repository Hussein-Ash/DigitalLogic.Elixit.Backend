using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.UserStore;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface IUserStoreService
{
    Task<(List<UserStoreDto>? dtos, int? totalCount, string? error)> GetAll(UserStoreFilter filter);

    Task<(UserStoreDto? dto, string? error)> Add(UserStoreForm form);

    Task<(UserStoreDto? dto, string? error)> Update(Guid id, UserStoreForm update);

    Task<(UserStoreDto? Dto, string? error)> Delete(Guid id);
    Task<(UserStoreDto? Dto, string? error)> GetById(Guid id);

}


public class UserStoreService : IUserStoreService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IUserClaimsService _claim;


    public UserStoreService(IMapper mapper, DataContext context, IUserClaimsService claim)
    {
        _mapper = mapper;
        _context = context;
        _claim = claim;
    }

    public async Task<(UserStoreDto? dto, string? error)> Add(UserStoreForm form)
    {
        var storeId = _claim.GetStoreId();
        var userStore = _mapper.Map<UserStore>(form);
        var user = await _context.Users.AnyAsync(x => x.Id == form.UserId && !x.Deleted);
        if (!user)
            return (null, "User not found");

        var store = await _context.Stores.AnyAsync(x => x.Id == storeId && !x.Deleted);
        if (!store)
            return (null, "Store not found");

        var result = (await _context.UserStores.AddAsync(userStore)).Entity;
        if (result == null)
            return (null, "Error while adding user store");
        await _context.SaveChangesAsync();
        return (_mapper.Map<UserStoreDto>(result), null);
    }

    public async Task<(UserStoreDto? Dto, string? error)> Delete(Guid id)
    {
        var userStore = await _context.UserStores.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (userStore == null)
            return (null, "User store not found");
        userStore.Deleted = true;
        _context.UserStores.Update(userStore);
        await _context.SaveChangesAsync();
        return (_mapper.Map<UserStoreDto>(userStore), null);
    }

    public async Task<(List<UserStoreDto>? dtos, int? totalCount, string? error)> GetAll(UserStoreFilter filter)
    {
        var query = _context.UserStores
            .AsNoTracking()
            .Where(x => !x.Deleted);
        var totalCount = await query.CountAsync();
        var dtos = await query
        .Paginate(filter)
            .ProjectTo<UserStoreDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(UserStoreDto? Dto, string? error)> GetById(Guid id)
    {
        var content = await _context.UserStores.FirstOrDefaultAsync(x=>x.Id == id);
        if (content == null) return (null, "not found");
        var contentDto = _mapper.Map<UserStoreDto>(content);
        return (contentDto, null);
    }

    public async Task<(UserStoreDto? dto, string? error)> Update(Guid id, UserStoreForm update)
    {
        var userStore = await _context.UserStores.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (userStore == null)
            return (null, "User store not found");
        _mapper.Map(update, userStore);
        _context.UserStores.Update(userStore);
        await _context.SaveChangesAsync();
        return (_mapper.Map<UserStoreDto>(userStore), null);
    }
}
