using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NotificationService.API.Entities;

public class NotificationReceived
{
    public DateTime CreationDate { get; set; }
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