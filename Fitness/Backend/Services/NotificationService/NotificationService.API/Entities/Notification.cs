using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.API.Entities;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public DateTime CreationDate { get; set; }
    public string UserId;
    public string Title { get; set; }
    public string Content { get; set; }
    public string Type { get; set; }
    public bool Email { get; set; }
    public bool NotificationRead { get; set; } = false;
}