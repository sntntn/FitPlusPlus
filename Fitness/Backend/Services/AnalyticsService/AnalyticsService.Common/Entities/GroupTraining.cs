using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Common.Entities;

public class GroupTraining
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ReservationId { get; set; }
    public string? TrainerId { get; set; }
    public List<string> ClientIds { get; set; }
    public int? Capacity { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? Date { get; set; }
    public Review? TrainerReview { get; set; }
    public List<Review>? ClientReviews { get; set; }
    public GroupTrainingStatus Status { get; set; } = GroupTrainingStatus.Active;
}

public enum GroupTrainingStatus
{
    Active,
    Removed
}
