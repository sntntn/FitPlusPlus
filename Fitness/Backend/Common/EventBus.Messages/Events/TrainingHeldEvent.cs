namespace EventBus.Messages.Events;

public enum TrainingStatus
{
    HELD,
    CANCELLED
}

public class TrainingHeldEvent : IntegrationBaseEvent
{
    public string TrainerId { get; set; }
    public string ClientId { get; set; }
    public DateTime TrainingDate { get; set; }
    public TrainingStatus Status { get; set; }
    public int TrainerRating { get; set; }
    public string TrainerComment { get; set; }
    public int ClientRating { get; set; }
    public string ClientComment { get; set; }
}