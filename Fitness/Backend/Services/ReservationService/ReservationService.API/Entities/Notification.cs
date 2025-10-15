using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.API.Entities;

public class Notification
{
    public IDictionary<string, string> UserIdToUserType;
    public string Title { get; set; }
    public string Content { get; set; }
    public NotificationType Type { get; set; }
    public bool Email { get; set; }

    public enum NotificationType
    {
        Information,
        Warning
    }
}