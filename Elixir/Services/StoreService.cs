using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.Store;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface IStoreService
{
    Task<(List<StoreDto>? dtos, int? totalCount, string? error)> GetAll(StoreFilter filter);
    Task<(Wallet? dto, string? error)> GetWallet(Guid storeId);

    Task<(StoreDto? dto, string? error)> Add(StoreForm form, Guid userId);

    Task<(StoreDto? dto, string? error)> Update(Guid id, StoreUpdate update, Guid userId);

    Task<(StoreDto? Dto, string? error)> Delete(Guid id, Guid userId);

    Task<(StoreDto? Dto, string? error)> GetById(Guid id);

}

public class StoreService : IStoreService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IUserClaimsService _claims;

    public StoreService(IMapper mapper, DataContext context, IUserClaimsService claims)
    {
        _mapper = mapper;
        _context = context;
        _claims = claims;
    }

    public async Task<(StoreDto? dto, string? error)> Add(StoreForm form, Guid userId)
    {
        var store = _mapper.Map<Store>(form);
        store.OwnerId = userId;
        store.Employees = [new UserStore { UserId = userId, Role = EmployeeRole.Owner }];

        var result = (await _context.Stores.AddAsync(store)).Entity;
        if (result == null)
            return (null, "Error while adding store");

        await _context.SaveChangesAsync();
        var Wallet = new Wallet
        {
            StoreId = store.Id,
            Amount = 0
        };
        var resultWallet = (await _context.Wallets.AddAsync(Wallet)).Entity;
        if (resultWallet == null)
            return (null, "Error while adding Wallet ");

        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreDto>(result), null);
    }

    public async Task<(StoreDto? Dto, string? error)> Delete(Guid id, Guid userId)
    {
        var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.OwnerId == userId);
        if (store == null)
            return (null, "Store not found");
        store.Deleted = true;
        store.Wallet.Deleted = true;
        _context.Stores.Update(store);
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreDto>(store), null);
    }

    public async Task<(List<StoreDto>? dtos, int? totalCount, string? error)> GetAll(StoreFilter filter)
    {
        var userId = _claims.GetUserId();
        var query = _context.Stores
        .AsNoTracking()
        .Include(x => x.Employees)
        .Where(w => !w.Deleted && (w.OwnerId == userId || w.Employees.Any(x => x.UserId == userId)));

        var totalCount = await query.CountAsync();
        var stores = await query
        .Paginate(filter)
        .ProjectTo<StoreDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return (stores, totalCount, null);
    }

    public async Task<(StoreDto? Dto, string? error)> GetById(Guid id)
    {
        var content = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
        if (content == null) return (null, "not found");
        var contentDto = _mapper.Map<StoreDto>(content);
        return (contentDto, null);
    }

    public async Task<(Wallet? dto, string? error)> GetWallet(Guid storeId)
    {
        var store = await _context.Stores.FirstOrDefaultAsync(x=>x.Id == storeId);
        if (store == null) return (null, "store not found");
        var wallet = await _context.Wallets.FirstOrDefaultAsync(x=>x.StoreId == storeId);
        if (wallet == null) return (null, "Wallet not found");
        return(wallet,null);
    }

    public async Task<(StoreDto? dto, string? error)> Update(Guid id, StoreUpdate update, Guid userId)
    {
        var store = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.OwnerId == userId);
        if (store == null)
            return (null, "Store not found");

        _mapper.Map(update, store);
        _context.Stores.Update(store);
        await _context.SaveChangesAsync();
        return (_mapper.Map<StoreDto>(store), null);
    }
}
