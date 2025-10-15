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
}

public enum GroupReservationEventType
{
    Added,
    Removed,
    ClientBooked,
    ClientCancelled,
}