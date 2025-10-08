namespace EventBus.Messages.Events;

public class GroupReservationEvent : IntegrationBaseEvent
{
    public string ReservationId { get; set; }
    public string? ClientId { get; set; }
    public string? TrainerId { get; set; }
    public string? TrainingName { get; set; }
    public int? Capacity { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? Date { get; set; }
    public GroupReservationEventType EventType { get; set; }
    
    /*
     * If the GroupTraining is added, the following data is sent:
     *  - ReservationId
     *  - TrainerId
     *  - TrainingName
     *  - Capacity
     *  - StartTime
     *  - EndTime
     *  - Date
     *  - EventType
     *
     * If the GroupTraining is removed, the following data is sent:
     *  - ReservationId
     *  - TrainerId
     *  - EventType
     *
     * If the GroupTraining is booked/canceled by client, the following data is sent:
     *  - ReservationId
     *  - ClientId
     *  - EventType
     */
}

public enum GroupReservationEventType
{
    Added,
    Removed,
    ClientBooked,
    ClientCancelled,
}