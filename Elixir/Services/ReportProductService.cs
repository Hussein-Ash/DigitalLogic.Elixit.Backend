using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.ReportProducts;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface IReportProductService
{
    Task<(List<ReportProductDto>? dtos, int? totalCount, string? error)> GetAll(ReportProductFilter filter);

    Task<(ReportProductDto? dto, string? error)> Add(ReportProductForm form,Guid userId);

    Task<(ReportProductDto? Dto, string? error)> Delete(Guid reportId);

    Task<(ReportProductDto? Dto, string? error)> AdminResponse(Guid id ,ReportProductUpdate update ,Guid AdminId);
}
public class ReportProductService : IReportProductService
{
    private readonly IUserClaimsService _claims;
    private readonly DataContext _db;
    private readonly IMapper _mapper;

    public ReportProductService(IMapper mapper, DataContext db, IUserClaimsService claims)
    {
        _mapper = mapper;
        _db = db;
        _claims = claims;
    }

    public async Task<(ReportProductDto? dto, string? error)> Add(ReportProductForm form,Guid userId)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x=>x.Id == userId);
        if(user == null) return (null,"user not found");

        var product = await _db.Products.FirstOrDefaultAsync(x=>x.Id == form.ProductId);
        if(product == null) return (null,"product not found");

        var newReport = new ReportProduct
        {
            ProductId = form.ProductId,
            UserId = userId,
            Status = ReportStatus.pending,
            Reason = form.Reason
        };
        await _db.ReportProducts.AddAsync(newReport);
        await _db.SaveChangesAsync();
        var reportDto = _mapper.Map<ReportProductDto>(newReport);
        return(reportDto,null);
    }

    public async Task<(ReportProductDto? Dto, string? error)> AdminResponse(Guid id, ReportProductUpdate update ,Guid adminId)
    {
        var report = await _db.ReportProducts.FirstOrDefaultAsync(x=>x.Id == id);
        if(report == null) return(null,"Reported Product not found");
        
        var admin = await _db.Users.FirstOrDefaultAsync(x=>x.Id == adminId);
        if(admin == null) return (null,"Admin not found");

        report.AdminId = adminId;
        report.Status = update.Status;
        report.AdminNote = update.AdminNote;
        _db.ReportProducts.Update(report);
        await _db.SaveChangesAsync();
        var reportDto = _mapper.Map<ReportProductDto>(report);
        return(reportDto,null);
        
        
    }

    public async Task<(ReportProductDto? Dto, string? error)> Delete(Guid reportId)
    {
        var report = await _db.ReportProducts.FirstOrDefaultAsync(x=>x.Id == reportId);
        if(report == null) return(null,"Reported Product Not Found");
        report.Deleted = true;
        _db.ReportProducts.Update(report);
        await _db.SaveChangesAsync();
        var reportDto = _mapper.Map<ReportProductDto>(report);
        return(reportDto,null);
    }

    public async Task<(List<ReportProductDto>? dtos, int? totalCount, string? error)> GetAll(ReportProductFilter filter)
    {
        var query = _db.ReportProducts.Where(x => !x.Deleted && (filter.Status == null || x.Status == filter.Status ));
        var totalCount = await query.CountAsync();
        var reports = await query.Paginate(filter)
        .ProjectTo<ReportProductDto>(_mapper.ConfigurationProvider)
        .ToListAsync();
        return (reports, totalCount, null);
    }
}
