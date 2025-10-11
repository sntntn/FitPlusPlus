using System.Text.Json;
using EventBus.Messages.Events;
using MassTransit;

namespace AnalyticsService.API.EventBusConsumers;

public class GroupReservationConsumer : IConsumer<GroupReservationEvent>
{
    public Task Consume(ConsumeContext<GroupReservationEvent> context)
    {
        var groupReservation = context.Message;
        Console.WriteLine(JsonSerializer.Serialize(groupReservation));
        // TODO(Handle reservation)
        return Task.CompletedTask;
    }
}