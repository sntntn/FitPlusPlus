using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.API.Entities;

public class NotificationReceived
{
    public DateTime CreationDate { get; set; }
    public string UserId;
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