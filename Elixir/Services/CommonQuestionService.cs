using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.CommonQuestions;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface ICommonQuestionService
{
    Task<(List<CommonQuestionDto>? dtos, int? totalCount, string? error)> GetAll(CommonQuestionFilter filter);

    Task<(CommonQuestionDto? dto, string? error)> Add(CommonQuestionForm form);

    Task<(CommonQuestionDto? dto, string? error)> Update(Guid id, CommonQuestionUpdate update);

    Task<(CommonQuestionDto? Dto, string? error)> Delete(Guid id);

    Task<(CommonQuestionDto? Dto, string? error)> GetById(Guid id);
}

public class CommonQuestionService : ICommonQuestionService
{
    private readonly DataContext _dbContext;
    private readonly IMapper _mapper;
    public CommonQuestionService(IMapper mapper
    , DataContext context)
    {
        _mapper = mapper;
        _dbContext = context;

    }

    public async Task<(CommonQuestionDto? dto, string? error)> Add(CommonQuestionForm form)
    {
        var newCommonQ = _mapper.Map<CommonQuestion>(form);
        await _dbContext.CommonQuestions.AddAsync(newCommonQ);
        await _dbContext.SaveChangesAsync();
        var commonQDto = _mapper.Map<CommonQuestionDto>(newCommonQ);
        return (commonQDto, null);
    }


    public async Task<(List<CommonQuestionDto>? dtos, int? totalCount, string? error)> GetAll(CommonQuestionFilter filter)
    {
        var query = _dbContext.CommonQuestions
            .AsNoTracking()
            .Where(x => !x.Deleted);
        var totalCount = await query.CountAsync();
        var dtos = await query
        .Paginate(filter)
            .ProjectTo<CommonQuestionDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return (dtos, totalCount, null);
    }

    public async Task<(CommonQuestionDto? Dto, string? error)> GetById(Guid id)
    {
        var commonQuestion = await _dbContext.CommonQuestions.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted);
        if (commonQuestion == null)
            return (null, "commonQuestion not found");
        var commonQDto = _mapper.Map<CommonQuestionDto>(commonQuestion);
        return (commonQDto, null);
    }


    public async Task<(CommonQuestionDto? dto, string? error)> Update(Guid id, CommonQuestionUpdate update)
    {
        var existingCommonQ = await _dbContext.CommonQuestions.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (existingCommonQ == null) return (null, "Product not found");
        _mapper.Map(update, existingCommonQ);
        _dbContext.CommonQuestions.Update(existingCommonQ);
        await _dbContext.SaveChangesAsync();
        var commonQDto = _mapper.Map<CommonQuestionDto>(existingCommonQ);
        return (commonQDto, null);
    }

    public async Task<(CommonQuestionDto? Dto, string? error)> Delete(Guid id)
    {
        var existingCommonQ = await _dbContext.CommonQuestions.FirstOrDefaultAsync(x => x.Id == id && x.Deleted == false);
        if (existingCommonQ == null) return (null, "not found");
        existingCommonQ.Deleted = true;
        _dbContext.CommonQuestions.Update(existingCommonQ);
        await _dbContext.SaveChangesAsync();
        return (_mapper.Map<CommonQuestionDto>(existingCommonQ), null);
    }
}

