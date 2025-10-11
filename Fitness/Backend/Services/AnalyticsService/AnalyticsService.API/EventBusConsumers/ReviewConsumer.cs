using System.Text.Json;
using EventBus.Messages.Events;
using MassTransit;

namespace AnalyticsService.API.EventBusConsumers;

public class ReviewConsumer : IConsumer<ReviewEvent>
{
    public Task Consume(ConsumeContext<ReviewEvent> context)
    {
        var review = context.Message;
        Console.WriteLine(JsonSerializer.Serialize(review));
        // TODO(Handle reservation)
        return Task.CompletedTask;
    }
}