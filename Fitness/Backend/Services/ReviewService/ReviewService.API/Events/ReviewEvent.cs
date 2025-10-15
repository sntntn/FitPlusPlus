namespace ReviewService.Common.Entities;

public class ReviewEvent
{
    public string ReservationId { get; set; }
    public string? UserId { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }
    public ReviewEventType EventType { get; set; }
}

public enum ReviewEventType
{
    ClientReview,
    TrainerReview
}