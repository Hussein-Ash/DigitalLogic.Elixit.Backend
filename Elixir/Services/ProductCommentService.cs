using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.ProductComment;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface IProductCommentService
{
    Task<(List<ProductCommentDto>? dtos, int? totalCount, string? error)> GetAll(ProductCommentFilter filter);

    Task<(ProductCommentDto? dto, string? error)> Add(ProductCommentForm form, Guid userId);

    Task<(ProductCommentDto? dto, string? error)> Reply(ProductReplyForm form, Guid userId);

    Task<(ProductCommentDto? dto, string? error)> Update(Guid id, ProductCommentUpdate updatem, Guid userId);

    Task<(ProductCommentDto? Dto, string? error)> Delete(Guid id, Guid userId);

    Task<(ProductCommentDto? Dto, string? error)> GetById(Guid id);


}
public class ProductCommentService : IProductCommentService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ProductCommentService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }

    public async Task<(ProductCommentDto? dto, string? error)> Add(ProductCommentForm form, Guid userId)
    {
        var productComment = _mapper.Map<ProductComment>(form);
        productComment.UserId = userId;
        var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == form.ProductId && !x.Deleted);
        if (product == null)
            return (null, "Product not found");
        product.Comment(1, 0);
        var result = (await _context.ProductComments.AddAsync(productComment)).Entity;
        if (result == null)
            return (null, "Error while adding product comment");
        await _context.SaveChangesAsync();


        return (_mapper.Map<ProductCommentDto>(result), null);
    }

    public async Task<(ProductCommentDto? Dto, string? error)> Delete(Guid id, Guid userId)
    {
        var productComment = await _context.ProductComments.Include(x=>x.ReplyComments).Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.UserId == userId);
        if (productComment == null)
            return (null, "Product comment not found");

        productComment.Deleted = true;
        productComment.Product?.Comment(0, 1);
        if (productComment.ParentId == null)
        {
            var replyComments = await _context.ProductComments.Where(x=>x.ParentId == productComment.Id && !x.Deleted).ToListAsync();
            replyComments.ForEach(x =>
            {
                x.Deleted = true;
                x.Product?.Comment(0, 1);
            });
        }
        else if (productComment.ParentId != null)
        {
            var mainComment = await _context.ProductComments.FirstOrDefaultAsync(x => x.Id == productComment.ParentId && !x.Deleted) ;
            if (mainComment == null) return (null, "the main comment not found");
            mainComment.Reply(0, 1);
        }

        _context.ProductComments.Update(productComment);
        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductCommentDto>(productComment), null);
    }

    public async Task<(List<ProductCommentDto>? dtos, int? totalCount, string? error)> GetAll(ProductCommentFilter filter)
    {
        var query = _context.ProductComments
        .Include(x => x.ReplyComments)
        .Include(x => x.User)
        .Where(x => !x.Deleted && x.ParentId == null && (filter.ProjectId == null || x.ProductId == filter.ProjectId));


        var totalCount = await query.CountAsync();

        var dtos = await query
            .Paginate(filter)
            .ProjectTo<ProductCommentDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        dtos.ForEach(async f =>
        {
            f.ReplyComments = await _context.ProductComments.Where(w => w.ParentId == f.Id).ProjectTo<ProductCommentDto>(_mapper.ConfigurationProvider).ToListAsync();

        });


        return (dtos, totalCount, null);
    }

    public async Task<(ProductCommentDto? Dto, string? error)> GetById(Guid id)
    {
        var content = await _context.ProductComments.Include(x => x.ReplyComments)
                .Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id && !x.Deleted) ;
        if (content == null) return (null, "not found");
        content.ReplyComments = await _context.ProductComments.Where(x=>x.ParentId == content.Id).ToListAsync();
        var contentDto = _mapper.Map<ProductCommentDto>(content);
        return (contentDto, null);
    }

    public async Task<(ProductCommentDto? dto, string? error)> Reply(ProductReplyForm form, Guid userId)
    {
        var comment = await _context.ProductComments.Include(x => x.Product).FirstOrDefaultAsync(x => x.Id == form.CommentId && !x.Deleted);
        if (comment == null) return (null, "comment not found");


        var newReply = new ProductComment
        {
            UserId = userId,
            ParentId = form.CommentId,
            Content = form.Content,
            ProductId = comment.ProductId
        };
        await _context.ProductComments.AddAsync(newReply);
        comment.Product?.Comment(1, 0);
        comment.Reply(1, 0);
        _context.ProductComments.Update(comment);

        await _context.SaveChangesAsync();
        var contentDto = _mapper.Map<ProductCommentDto>(newReply);
        return (contentDto, null);

    }

    public async Task<(ProductCommentDto? dto, string? error)> Update(Guid id, ProductCommentUpdate update, Guid userId)
    {
        var productComment = await _context.ProductComments.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.UserId == userId);
        if (productComment == null)
            return (null, "Product comment not found");
        _mapper.Map(update, productComment);
        _context.ProductComments.Update(productComment);
        await _context.SaveChangesAsync();
        return (_mapper.Map<ProductCommentDto>(productComment), null);
    }



}
