using System;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Elixir.DATA;
using Elixir.DATA.DTOs.Like;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;
public interface ILikeService
{
    Task<(List<LikeDto>? dtos, int? totalCount, string? error)> GetAllLikes(LikeFilter filter);
    Task<(LikeDto? dto, string? error)> AddLike(LikeForm form, Guid id);
    Task<(LikeDto? Dto, string? error)> RemoveLike(Guid id);

}

public class LikeService : ILikeService
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public LikeService(IMapper mapper, DataContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    public async Task<(LikeDto? dto, string? error)> AddLike(LikeForm form, Guid id)
    {
        var Like = await _context.Likes.FirstOrDefaultAsync(x => x.UserId == id && (x.CommentId == form.LikedId || x.ProductId == form.LikedId) && !x.Deleted);
        if (Like != null) return (null, "u already Liked this");

        if (form.LikedId != null)
        {
            var newLike = new Like();
            if (form.Type == LikeType.Comment)
            {
                var comment = await _context.ProductComments.FirstOrDefaultAsync(x => x.Id == form.LikedId);
                if (comment == null) return (null, "comment not found");
                newLike = new Like
                {
                    UserId = id,
                    CommentId = form.LikedId,
                    Type = form.Type

                };
                comment.Like(1,0);

            }
            else if (form.Type == LikeType.Product)
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == form.LikedId);
                if (product == null) return (null, "comment not found");
                newLike = new Like
                {
                    UserId = id,
                    ProductId = form.LikedId,
                    Type = form.Type

                };
                product.Like(1,0);



            }
            else return (null, "Empty data");
            

            await _context.Likes.AddAsync(newLike);
            await _context.SaveChangesAsync();
            return (_mapper.Map<LikeDto>(newLike), null);
        }

        return (null, "Empty data");
    }

    public async Task<(List<LikeDto>? dtos, int? totalCount, string? error)> GetAllLikes(LikeFilter filter)
    {
        var query = _context.Likes.Include(x => x.Product).Include(x => x.Comment)
            .Where(x => !x.Deleted &&
            (filter.CommentId == null || x.CommentId == filter.CommentId) && (filter.ProductId == null || x.ProductId == filter.ProductId));
        var totalCount = await query.CountAsync();
        var likes = await query
        .Paginate(filter)
        .ProjectTo<LikeDto>(_mapper.ConfigurationProvider)
        .ToListAsync();

        return (likes, totalCount, null);
    }

    public async Task<(LikeDto? Dto, string? error)> RemoveLike(Guid LikeId)
    {
        var like = await _context.Likes.Include(x=>x.Comment).Include(y=>y.Product).FirstOrDefaultAsync(x => x.Id == LikeId && !x.Deleted);
        if (like == null) return (null, "you do not have like on it ");
        like.Comment?.Like(0,1);
        like.Product?.Like(0,1);

        like.Deleted = true;
        _context.Likes.Update(like);
        await _context.SaveChangesAsync();
        return (_mapper.Map<LikeDto>(like), null);

    }

    
}
