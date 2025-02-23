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

    public async Task PublishNotification(string title, string content, string type, bool email, string userId, string userType)
    {
        Enum.TryParse(userType, out Notification.UserType uType);
        Enum.TryParse(type, out Notification.NotificationType nType);
        var notification = new Notification
        {
            Title = title,
            Content = content,
            NType = nType,
            Email = email,
            UserId = userId,
            UType = uType
        };

        var eventMessage = _mapper.Map<NotificationEvent>(notification);
        await _publishEndpoint.Publish(eventMessage);
    }
}