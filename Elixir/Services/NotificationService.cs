using System;
using System.Reactive;
using Elixir.DATA;
using Elixir.DATA.DTOs;
using Elixir.Entities;
using Elixir.Helpers;
using Microsoft.EntityFrameworkCore;

namespace Elixir.Services;

public interface INotificationService
{
    Task<(List<Notifications>? dtos, int? totalCount, string? error)> GetAll(BaseFilter filter, Guid userId);

    Task<(Notifications? Dto, string? error)> Delete(Guid id, Guid userId);
}

public class NotificationService : INotificationService
{
    private readonly DataContext _context;

    public NotificationService(DataContext context)
    {
        _context = context;
    }

    public async Task<(Notifications? Dto, string? error)> Delete(Guid id, Guid userId)
    {
        var notification = await _context.Notifications.FirstOrDefaultAsync(x => x.Id == id && !x.Deleted && x.UserId == userId);
        if (notification == null)
        {
            return (null, "Notification not found");
        }
        notification.Deleted = true;
        _context.Notifications.Update(notification);
        await _context.SaveChangesAsync();
        return (notification, null);
    }

    public async Task<(List<Notifications>? dtos, int? totalCount, string? error)> GetAll(BaseFilter filter, Guid userId)
    {
        var Notifications = _context.Notifications
          .Where(x => x.UserId == userId);
        var totalCount = await Notifications.CountAsync();
        var dtos = await Notifications
            .OrderByDescending(x => x.CreatedAt)
            .Paginate(filter)
            .ToListAsync();
        return (dtos, totalCount, null);

    }
}
