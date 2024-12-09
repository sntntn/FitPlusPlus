using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Common.Entities;

public class Training
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string TrainerId { get; set; }
    
    [BsonRepresentation(BsonType.ObjectId)]
    public string ClientId { get; set; }
    
    public DateTime TrainingDate { get; set; }
    
    public TrainingStatus Status { get; set; }
    
    public int Rating { get; set; }
}