using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace AnalyticsService.Common.Entities;

public class Training
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string TrainerId { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string ClientId { get; set; }
    public DateTime TrainingDate { get; set; }
    
    public TrainingStatus Status { get; set; }
    
    public int TrainerRating { get; set; }
    public string TrainerComment { get; set; }
    
    public int ClientRating { get; set; }
    public string ClientComment { get; set; }
}