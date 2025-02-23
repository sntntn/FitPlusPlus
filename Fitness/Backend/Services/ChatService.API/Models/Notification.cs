namespace ChatService.API.Models;

public class Notification
{
    public string UserId { get; set; }
    public UserType UType { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public NotificationType NType { get; set; }
    public bool Email { get; set; }

    public enum UserType
    {
        Client,
        Trainer
    }
    
    public enum NotificationType
    {
        Information,
        Warning
    }
}