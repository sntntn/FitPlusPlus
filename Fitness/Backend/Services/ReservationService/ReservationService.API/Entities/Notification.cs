using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.API.Entities;

public class Notification
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public Guid Id { get; set; }
    public DateTime CreationDate { get; set; }
    public IDictionary<string, string> UserIdToUserType;
    public string Title { get; set; }
    public string Content { get; set; }
    public NotificationType Type { get; set; }
    public bool Email { get; set; }
    public bool NotificationRead { get; set; } = false;

    public enum NotificationType
    {
        Information,
        Warning
    }
}