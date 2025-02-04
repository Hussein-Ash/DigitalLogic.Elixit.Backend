using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Elixir.Helpers.OneSignal;
using System.Reactive;
using Elixir.Entities;
using Elixir.DATA;


public class OrderPlacedNotificationHandler : INotificationHandler<OrderPlacedNotification>
{
    private readonly DataContext _context;

    public OrderPlacedNotificationHandler(DataContext context)
    {
        _context = context;
    }

    public async Task Handle(OrderPlacedNotification order, CancellationToken cancellationToken)
    {
        OneSignal.SendNoitications(order.Notification, order.to ?? "All");

        Console.WriteLine($"Order placed: {order.Notification.ToString} for {order.to ?? "All"}");

        await _context.Notifications.AddAsync(order.Notification);
        await _context.SaveChangesAsync();

    }
}
