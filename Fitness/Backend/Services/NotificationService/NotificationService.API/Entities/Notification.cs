namespace NotificationService.API.Entities;

public class Notification
{
    public Guid Id { get; set; }
    private DateTime CreationDate { get; set; }
    public string UserType { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public NotificationType Type { get; set; }
    public bool Email { get; set; }
    public bool Push { get; set; }
    public bool NotificationRead { get; set; } = false;

    public enum NotificationType
    {
        Information,
        Warning
    }
}