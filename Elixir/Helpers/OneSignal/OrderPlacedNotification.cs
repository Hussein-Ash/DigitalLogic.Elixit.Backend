using System.Reactive;
using Elixir.Entities;
using MediatR;

public class OrderPlacedNotification( Notifications notification, string? to ) : INotification
{
    // public Guid? OrderId { get; } = orderId;
    public string? to { get; } = to;
    public Notifications Notification { get; } = notification;
}
