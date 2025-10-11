using AutoMapper;
using MassTransit;
using ReservationService.API.Entities;

namespace ReservationService.API.Publishers;

public class IndividualReservationPublisher : IIndividualReservationPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public IndividualReservationPublisher(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public Task PublishBooked(IndividualReservation individualReservation)
    {
        var booked = new IndividualReservationEvent
        {
            ReservationId = individualReservation.Id,
            ClientId = individualReservation.ClientId,
            TrainerId = individualReservation.TrainerId,
            TrainingTypeId = individualReservation.TrainingTypeId,
            StartTime = individualReservation.StartTime,
            EndTime = individualReservation.EndTime,
            Date = individualReservation.Date,
            EventType = IndividualReservationEventType.Booked
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.IndividualReservationEvent>(booked);
        return _publishEndpoint.Publish(eventMessage);
    }

    public Task PublishClientCancelled(IndividualReservation individualReservation)
    {
        var cancelled = new IndividualReservationEvent
        {
            ReservationId = individualReservation.Id,
            ClientId = individualReservation.ClientId,
            EventType = IndividualReservationEventType.CancelledByClient
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.IndividualReservationEvent>(cancelled);
        return _publishEndpoint.Publish(eventMessage);
    }

    public Task PublishTrainerCancelled(IndividualReservation individualReservation)
    {
        var cancelled = new IndividualReservationEvent
        {
            ReservationId = individualReservation.Id,
            TrainerId = individualReservation.TrainerId,
            EventType = IndividualReservationEventType.CancelledByTrainer
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.IndividualReservationEvent>(cancelled);
        return _publishEndpoint.Publish(eventMessage);
    }
}