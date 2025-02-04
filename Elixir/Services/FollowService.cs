using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.Follow;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface IFollowService
{
    Task<(List<FollowDto>? dtos, int? totalCount, string? error)> GetAllFollowers(FollowFilter filter, Guid id);
    Task<(List<FollowDto>? dtos, int? totalCount, string? error)> GetAllFollowings(FollowFilter filter, Guid id);
    Task<(FollowDto? dto, string? error)> AddFollow(FollowForm form, Guid id);
    Task<(FollowDto? Dto, string? error)> RemoveFollowing(Guid id, Guid userId);
    Task<(FollowDto? Dto, string? error)> RemoveFollower(Guid id, Guid userId);


}

public class FollowService : IFollowService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public FollowService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(FollowDto? dto, string? error)> AddFollow(FollowForm form, Guid id)
    {
        var following = await _context.Follows.FirstOrDefaultAsync(x => (x.UserId == id || x.StoreId == id) && x.FollowedStoreId == form.FollowingId && !x.Deleted);
        if (following != null) return (null, "u already follow this");

        var followedStore = await _context.Stores.FirstOrDefaultAsync(x => x.Id == form.FollowingId);
        if (followedStore == null) return (null, "store not found");

        if (form.FollowingId != null)
        {
            var newFollow = new Follow();
            if (form.Follower == FollowerType.User)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x=>x.Id == id);
                if(user == null) return (null,"user not found");
                if(form.FollowingId == user.Id) return(null,"you cant follower users");

                newFollow = new Follow
                {
                    UserId = id,
                    FollowedStoreId = form.FollowingId
                };
                user.Following(1,0);
                
            }
            else if (form.Follower == FollowerType.Store)
            {
                var store = await _context.Stores.FirstOrDefaultAsync(x=>x.Id == id);
                if(store == null) return (null,"user not found");
                if (id == form.FollowingId) return (null, "You Cant Follow YourSelf");
                newFollow = new Follow
                {
                    StoreId = id,
                    FollowedStoreId = form.FollowingId
                };
                store.Following(1,0);
                

            }
            else return (null, "Empty data");
            followedStore.Follower(1,0);

            await _context.Follows.AddAsync(newFollow);
            await _context.SaveChangesAsync();
            return (_mapper.Map<FollowDto>(newFollow), null);
        }

        return (null, "Empty data");


    }

    public async Task<(List<FollowDto>? dtos, int? totalCount, string? error)> GetAllFollowers(FollowFilter filter, Guid id)
    {
        var query = _context.Follows.Include(x => x.User).Include(x => x.Store)
            .Where(x => !x.Deleted && x.UserId == id || x.StoreId == id);
        var totalCount = await query.CountAsync();
        var Orders = await query
        .Paginate(filter)
        .ProjectTo<FollowDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return (Orders, totalCount, null);
    }

    public async Task<(List<FollowDto>? dtos, int? totalCount, string? error)> GetAllFollowings(FollowFilter filter, Guid id)
    {
        var query = _context.Follows.Include(x => x.User).Include(x => x.Store)
            .Where(x => !x.Deleted && x.FollowedStoreId == id);
        var totalCount = await query.CountAsync();
        var Orders = await query
        .Paginate(filter)
        .ProjectTo<FollowDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return (Orders, totalCount, null);
    }

    public async Task<(FollowDto? Dto, string? error)> RemoveFollowing(Guid id, Guid userId)
    {
        var following = await _context.Follows.Include(x=>x.Store).Include(x=>x.User)
            .FirstOrDefaultAsync(x => (x.UserId == userId || x.StoreId == userId) && x.FollowedStoreId == id && !x.Deleted);
        if (following == null) return (null, "u already un followed that person");

        var followedStore = await _context.Stores.FirstOrDefaultAsync(x => x.Id == id);
        if (followedStore == null) return (null, "store not found");

        followedStore.Follower(0,1);
        following.Store?.Following(0,1);
        following.User?.Following(0,1);

        following.Deleted = true;
        _context.Follows.Update(following);
        await _context.SaveChangesAsync();

        return (_mapper.Map<FollowDto>(following), null);
    }

    public async Task<(FollowDto? Dto, string? error)> RemoveFollower(Guid id, Guid storeId)
    {
        var following = await _context.Follows.Include(x=>x.Store)
                    .Include(x=>x.User)
                    .Include(x=>x.FollowedStore)
            .FirstOrDefaultAsync(x => (x.UserId == id || x.StoreId == id) && x.FollowedStoreId == storeId && !x.Deleted);
        if (following == null) return (null, "already done");

        following.FollowedStore?.Follower(0,1);
        following.Store?.Following(0,1);
        following.User?.Following(0,1);


        following.Deleted = true;
        _context.Follows.Update(following);
        await _context.SaveChangesAsync();

        return (_mapper.Map<FollowDto>(following), null);
    }

    
}
