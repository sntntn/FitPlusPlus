namespace ChatService.API.Publishers;

public interface INotificationPublisher
{
    Task PublishNotification(string title, string content, string type, bool email, string userId, string userType);
}