using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.UserAddressDto;
using Elixir.Entities;
using Elixir.Helpers;
using Elixir.Repository;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface IUserAddressService
{

    Task<(UserAddressDto? userAddressDto, string? error)> Create(UserAddressForm userAddressForm);
    Task<(UserAddress? userAddress, string? error)> Delete(Guid id);
    Task<(UserAddressDto? userAddressDto, string? error)> Update(UserAddressUpdate update, Guid userAddressId);
    Task<(UserAddressDto? userAddressDto, string? error)> GetById(Guid id);
    Task<(List<UserAddressDto>? userAddressDtos, int? totalCount, string? error)> GetAll(UserAddressFilter filter);

}

public class UserAddressService : IUserAddressService
{
    private readonly DataContext _dbContext;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly IUserClaimsService _claim;

    public UserAddressService(IRepositoryWrapper repositoryWrapper, IMapper mapper
    , DataContext context, IUserClaimsService claim)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _dbContext = context;
        _claim = claim;
    }

    public async Task<(UserAddressDto? userAddressDto, string? error)> Create(UserAddressForm userAddressForm)
    {
        var newAddress = _mapper.Map<UserAddress>(userAddressForm);
        await _dbContext.UserAddresses.AddAsync(newAddress);
        await _dbContext.SaveChangesAsync();
        var addressDto = _mapper.Map<UserAddressDto>(newAddress);
        return (addressDto, null);
    }

    public async Task<(UserAddress? userAddress, string? error)> Delete(Guid id)
    {
        var address = await _repositoryWrapper.UserAddress.GetById(id);
        if (address == null) return (null, "already deleted");
        var deleteaddress = await _repositoryWrapper.UserAddress.SoftDelete(id);
        return (deleteaddress, null);

    }

    public async Task<(List<UserAddressDto>? userAddressDtos, int? totalCount, string? error)> GetAll(UserAddressFilter filter)
    {
        var userId = _claim.GetUserId();
        var query = _dbContext.UserAddresses
         .AsNoTracking()
         .Where(x => !x.Deleted && x.UserId == userId);
        var totalCount = await query.CountAsync();

        var dtos = await query
            .Paginate(filter)
            .ProjectTo<UserAddressDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(UserAddressDto? userAddressDto, string? error)> GetById(Guid id)
    {
        var address = await _repositoryWrapper.UserAddress.GetById(id);
        if (address == null) return (null, "not found");
        var addressDto = _mapper.Map<UserAddressDto>(address);
        return (addressDto, null);
    }

    public async Task<(UserAddressDto? userAddressDto, string? error)> Update(UserAddressUpdate update, Guid userAddressId)
    {
        var existingAddress = await _dbContext.UserAddresses.FirstOrDefaultAsync(x => x.Id == userAddressId && x.Deleted == false);
        if (existingAddress == null) return (null, "Address not found");
        _mapper.Map(update, existingAddress);
        _dbContext.UserAddresses.Update(existingAddress);
        await _dbContext.SaveChangesAsync();
        var AddressDto = _mapper.Map<UserAddressDto>(existingAddress);
        return (AddressDto, null);
    }
}
