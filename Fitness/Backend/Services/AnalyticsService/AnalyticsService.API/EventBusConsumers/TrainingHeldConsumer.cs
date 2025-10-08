using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;

namespace AnalyticsService.API.EventBusConsumers;

public class TrainingHeldConsumer : IConsumer<IndividualReservationEvent>
{
    private readonly IAnalyticsRepository _repository;
    private readonly IMapper _mapper;

    public TrainingHeldConsumer(IAnalyticsRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Consume(ConsumeContext<IndividualReservationEvent> context)
    {
        var training = _mapper.Map<Training>(context.Message);
        await _repository.CreateTraining(training);
    }
}