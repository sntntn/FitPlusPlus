using AutoMapper;
using MassTransit;
using ReservationService.API.Entities;

namespace ReservationService.API.Publishers;

public class GroupReservationPublisher : IGroupReservationPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public GroupReservationPublisher(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task PublishAdded(GroupReservation groupReservation)
    {
        var added = new GroupReservationEvent
        {
            ReservationId = groupReservation.Id,
            TrainerId = groupReservation.TrainerId,
            TrainingName = groupReservation.Name,
            Capacity = groupReservation.Capacity,
            StartTime = groupReservation.StartTime,
            EndTime = groupReservation.EndTime,
            Date = groupReservation.Date,
            EventType = GroupReservationEventType.Added
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.GroupReservationEvent>(added);
        await _publishEndpoint.Publish(eventMessage);
    }

    public async Task PublishRemoved(GroupReservation groupReservation)
    {
        var removed = new GroupReservationEvent
        {
            ReservationId = groupReservation.Id,
            TrainerId = groupReservation.TrainerId,
            EventType = GroupReservationEventType.Removed
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.GroupReservationEvent>(removed);
        await _publishEndpoint.Publish(eventMessage);
    }

    public async Task PublishBooked(GroupReservation groupReservation, string clientId)
    {
        var booked = new GroupReservationEvent
        {
            ReservationId = groupReservation.Id,
            ClientId = clientId,
            EventType = GroupReservationEventType.ClientBooked
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.GroupReservationEvent>(booked);
        await _publishEndpoint.Publish(eventMessage);
    }

    public async Task PublishCancelled(GroupReservation groupReservation, string clientId)
    {
        var cancelled = new GroupReservationEvent
        {
            ReservationId = groupReservation.Id,
            ClientId = clientId,
            EventType = GroupReservationEventType.ClientCancelled
        };
        var eventMessage = _mapper.Map<EventBus.Messages.Events.GroupReservationEvent>(cancelled);
        await _publishEndpoint.Publish(eventMessage);
    }
}