using AnalyticsService.Common.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Common.DTOs;

public class ClientTrainingDTO
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string TrainerId { get; set; }
    public DateTime TrainingDate { get; set; }
    public TrainingStatus Status { get; set; }
    public int TrainerRating { get; set; }
    public int ClientRating { get; set; }
}