using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Common.Entities;

public class IndividualTraining
{
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string? ReservationId { get; set; }
    public string? TrainerId { get; set; }
    public string? ClientId { get; set; }
    public string? TrainingTypeId { get; set; }
    public TimeOnly? StartTime { get; set; }
    public TimeOnly? EndTime { get; set; }
    public DateOnly? Date { get; set; }
    public Review? TrainerReview { get; set; }
    public Review? ClientReview { get; set; }
    public IndividualTrainingStatus Status { get; set; } = IndividualTrainingStatus.Active;
}

public enum IndividualTrainingStatus
{
    Active,
    TrainerCancelled,
    ClientCancelled
}
