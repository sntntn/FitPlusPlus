namespace EventBus.Messages.Events;

public class ReviewEvent : IntegrationBaseEvent
{
    public string ReservationId { get; set; }
    public string UserId { get; set; }
    public string Comment { get; set; }
    public int Rating { get; set; }
    public ReviewEventType EventType { get; set; }
    
    /*
     * When the client/trainer review the training, the following data is sent:
     *  - ReservationId
     *  - UserId
     *  - Comment
     *  - Rating
     *  - EventType -- which one has reviewed the training
     *
     * Possibly some kind of validation could be done on the AnalyticsService side.
     */
}

public enum ReviewEventType
{
    ClientReview,
    TrainerReview
}