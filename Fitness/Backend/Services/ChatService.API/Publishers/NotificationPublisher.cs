using AutoMapper;
using ChatService.API.Models;
using EventBus.Messages.Events;
using MassTransit;

namespace ChatService.API.Publishers;

public class NotificationPublisher : INotificationPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    public NotificationPublisher(IPublishEndpoint publishEndpoint, IMapper mapper)
    {
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task PublishNotification(string title, string content, string type, bool email, string userId)
    {
        Enum.TryParse(type, out Notification.NotificationType notificationType);
        var notification = new Notification
        {
            Title = title,
            Content = content,
            Type = notificationType,
            Email = email,
            UserId = userId
        };

        var eventMessage = _mapper.Map<NotificationEvent>(notification);
        await _publishEndpoint.Publish(eventMessage);
    }
}