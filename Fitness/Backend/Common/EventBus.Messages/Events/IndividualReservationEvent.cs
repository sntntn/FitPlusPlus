namespace EventBus.Messages.Events;

public class IndividualReservationEvent : IntegrationBaseEvent
{
    public string ReservationId { get; set; }
    public string? ClientId { get; set; }
    public string? TrainerId { get; set; }
    public string? TrainingTypeId { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? Date { get; set; }
    public IndividualReservationEventType EventType { get; set; }
}

public enum IndividualReservationEventType
{
    Booked,
    CancelledByClient,
    CancelledByTrainer,
}