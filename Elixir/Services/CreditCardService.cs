using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.CreditCard;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface ICreditCardService
{
    Task<(List<CreditCardDto>? dtos, int? totalCount, string? error)> GetAll(CreditCardFilter filter, Guid userId);

    Task<(CreditCardDto? dto, string? error)> Add(CreditCardForm form, Guid userId);

    Task<(CreditCardDto? dto, string? error)> Update(Guid id, CreditCardUpdate update, Guid userId);

    Task<(CreditCardDto? Dto, string? error)> Delete(Guid id, Guid userId);

    Task<(CreditCardDto? Dto, string? error)> GetById(Guid id, Guid userId);

}

public class CreditCardService : ICreditCardService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public CreditCardService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(CreditCardDto? dto, string? error)> Add(CreditCardForm form, Guid userId)
    {
        var creditCard = _mapper.Map<CreditCard>(form);
        creditCard.UserId = userId;
        var result = (await _context.CreditCards.AddAsync(creditCard)).Entity;

        if (result == null)
            return (null, "Failed to add credit card");

        await _context.SaveChangesAsync();
        return (_mapper.Map<CreditCardDto>(result), null);

    }

    public async Task<(CreditCardDto? Dto, string? error)> Delete(Guid id, Guid userId)
    {
        var creditCard = await _context.CreditCards.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.UserId == userId);
        if (creditCard == null)
            return (null, "Credit card not found");

        creditCard.Deleted = true;
        var result = _context.CreditCards.Update(creditCard).Entity;
        if (result == null)
            return (null, "Failed to delete credit card");
        await _context.SaveChangesAsync();
        return (_mapper.Map<CreditCardDto>(result), null);
    }

    public async Task<(List<CreditCardDto>? dtos, int? totalCount, string? error)> GetAll(CreditCardFilter filter, Guid userId)
    {
        var query = _context.CreditCards.AsNoTracking()
        .Where(x => !x.Deleted && x.UserId == userId);

        var totalCount = await query.CountAsync();

        var creditCards = await query.Paginate(filter)
        .ProjectTo<CreditCardDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return (creditCards, totalCount, null);
    }

    public async Task<(CreditCardDto? Dto, string? error)> GetById(Guid id, Guid userId)
    {
        var content = await _context.CreditCards.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (content == null)
            return (null, "credit card not found");
        var contentDto = _mapper.Map<CreditCardDto>(content);
        return (contentDto, null);
    }

    public async Task<(CreditCardDto? dto, string? error)> Update(Guid id, CreditCardUpdate update, Guid userId)
    {
        var creditCard = await _context.CreditCards.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.UserId == userId);
        if (creditCard == null)
            return (null, "Credit card not found");

        _mapper.Map(update, creditCard);
        var result = _context.CreditCards.Update(creditCard).Entity;
        if (result == null)
            return (null, "Failed to update credit card");
        await _context.SaveChangesAsync();
        return (_mapper.Map<CreditCardDto>(result), null);
    }
}
