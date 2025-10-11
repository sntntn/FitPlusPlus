using System.Text.Json;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;

namespace AnalyticsService.API.EventBusConsumers;

public class IndividualReservationConsumer : IConsumer<IndividualReservationEvent>
{
    public Task Consume(ConsumeContext<IndividualReservationEvent> context)
    {
        var individualReservation = context.Message;
        Console.WriteLine(JsonSerializer.Serialize(individualReservation));
        // TODO(Handle reservation)
        return Task.CompletedTask;
    }
}