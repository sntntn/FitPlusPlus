using MassTransit;

namespace ReservationService.API.Publishers;

public interface INotificationPublisher
{
    Task PublishNotification(string title, string content, string type, bool email, IDictionary<string, string> users);
}