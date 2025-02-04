using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.Product;
using Elixir.DATA.DTOs.UserSave;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface IUserSaveService
{
    Task<(List<ProductDto>? dtos, int? totalCount, string? error)> GetAll(UserSaveFilter filter);

    Task<(ProductDto? dto, string? error)> Add(UserSaveForm form);

    Task<(ProductDto? Dto, string? error)> Delete(Guid productId);

    Task<(ProductDto? Dto, string? error)> GetById(Guid id);

}

public class UserSaveService : IUserSaveService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    private readonly IUserClaimsService _claims;

    public UserSaveService(IUserClaimsService claims, IMapper mapper, DataContext context)
    {
        _claims = claims;
        _mapper = mapper;
        _context = context;
    }

    public async Task<(ProductDto? dto, string? error)> Add(UserSaveForm form)
    {
        var userId = _claims.GetUserId();
        var IsSaved = await _context.UserSaves.Include(x => x.User).FirstOrDefaultAsync(x => x.ProductId == form.ProductId && x.UserId == (Guid)userId);
        if (IsSaved == null) return (null, "Product already saved");

        var userSave = _mapper.Map<UserSave>(form);
        userSave.UserId = (Guid)userId;

        userSave.User.SavedProduct(1, 0);

        var result = (await _context.UserSaves.AddAsync(userSave)).Entity;
        if (result == null) return (null, "Error saving user save");
        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductDto>(result), null);
    }

    public async Task<(ProductDto? Dto, string? error)> Delete(Guid productId)
    {
        var userId = _claims.GetUserId();
        var userSave = await _context.UserSaves.Include(x => x.User).FirstOrDefaultAsync(x => x.ProductId == productId && !x.Deleted && x.UserId == (Guid)userId);
        if (userSave == null) return (null, "User save not found");

        userSave.Deleted = true;
        userSave.User.SavedProduct(0, 1);

        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductDto>(userSave), null);
    }

    public async Task<(List<ProductDto>? dtos, int? totalCount, string? error)> GetAll(UserSaveFilter filter)
    {
        var userId = _claims.GetUserId();
        var query = _context.UserSaves.Where(x => x.UserId == (Guid)userId && !x.Deleted);
        var totalCount = await query.CountAsync();
        var userSaves = await query.Paginate(filter)
        .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
        return (userSaves, totalCount, null);
    }

    public async Task<(ProductDto? Dto, string? error)> GetById(Guid id)
    {
        var userId = _claims.GetUserId();
        var userSave = await _context.UserSaves.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.UserId == (Guid)userId);
        if (userSave == null) return (null, "User save not found");
        return (_mapper.Map<ProductDto>(userSave), null);
    }
}
