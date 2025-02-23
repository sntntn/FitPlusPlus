namespace EventBus.Messages.Events;

public class NotificationEvent : IntegrationBaseEvent
{
    public string UserId { get; set; }
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