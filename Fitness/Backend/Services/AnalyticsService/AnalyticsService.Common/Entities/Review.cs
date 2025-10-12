using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Common.Entities;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ReservationId { get; set; }
    public string UserId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
}