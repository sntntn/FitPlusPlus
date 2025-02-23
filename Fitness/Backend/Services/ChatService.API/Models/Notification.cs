namespace ChatService.API.Models;

public class Notification
{
    public string UserId { get; set; }
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