using System.Text.Json;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using GroupReservationEventType = EventBus.Messages.Events.GroupReservationEventType;

namespace AnalyticsService.API.EventBusConsumers;

public class GroupReservationConsumer : IConsumer<GroupReservationEvent>
{
    private readonly IAnalyticsRepository _repository;

    public GroupReservationConsumer(IAnalyticsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    /// <summary>
    /// Method for processing <c>GroupReservationEvent</c> object based on the event type.
    /// In case of an event where a new group training was added,
    /// a new group training is also registered in Analytics service. Otherwise, the
    /// corresponding training to reflect the behavior in the Reservation service.
    /// </summary>
    /// <param name="context"><c>ConsumeContext</c> object representing the send message through the bus</param>
    public async Task Consume(ConsumeContext<GroupReservationEvent> context)
    {
        GroupReservationEvent groupReservation = context.Message;
        switch (groupReservation.EventType)
        {
            case GroupReservationEventType.Added:
            {
                GroupTraining groupTraining = new GroupTraining
                {
                    ReservationId = groupReservation.ReservationId,
                    TrainerId = groupReservation.TrainerId,
                    ClientIds = [],
                    Capacity = groupReservation.Capacity,
                    StartTime = groupReservation.StartTime,
                    EndTime = groupReservation.EndTime,
                    Date = groupReservation.Date,
                    TrainerReview = null,
                    ClientReviews = null,
                    Status = GroupTrainingStatus.Active
                };
                await _repository.CreateGroupTraining(groupTraining);
                break;
            }
            case GroupReservationEventType.Removed:
            {
                GroupTraining groupTraining =
                    await _repository.GetGroupTrainingByReservationId(groupReservation.ReservationId);
                groupTraining.Status = GroupTrainingStatus.Removed;
                await _repository.UpdateGroupTraining(groupTraining);
                break;
            }
            case GroupReservationEventType.ClientBooked:
            {
                GroupTraining groupTraining =
                    await _repository.GetGroupTrainingByReservationId(groupReservation.ReservationId);
                if (groupReservation.ClientId != null)
                {
                    groupTraining.ClientIds.Add(groupReservation.ClientId);
                    await _repository.UpdateGroupTraining(groupTraining);
                }
                break;
            }
            case GroupReservationEventType.ClientCancelled:
            {
                GroupTraining groupTraining =
                    await _repository.GetGroupTrainingByReservationId(groupReservation.ReservationId);
                if (groupReservation.ClientId != null)
                {
                    groupTraining.ClientIds.Remove(groupReservation.ClientId);
                    await _repository.UpdateGroupTraining(groupTraining);
                }
                break;
            }
        }
    }
}