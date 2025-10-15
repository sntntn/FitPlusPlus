namespace ChatService.API.Publishers;

public interface INotificationPublisher
{
    /// <summary>
    /// Publishes a notification event to the event bus.
    /// </summary>
    /// <param name="title">The title of the notification.</param>
    /// <param name="content">The main content of the notification message.</param>
    /// <param name="type">The type of notification (e.g., Info, Warning, Error).</param>
    /// <param name="email">Indicates whether an email should also be sent.</param>
    /// <param name="users">Dictionary mapping user IDs to user roles/types.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>

    Task PublishNotification(string title, string content, string type, bool email, IDictionary<string, string> users);
}