using AutoMapper;
using ChatService.API.Models;
using EventBus.Messages.Events;
using MassTransit;
using MongoDB.Bson;

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

    public async Task PublishNotification(string title, string content, string type, bool email,
        IDictionary<string, string> users)
    {
        Enum.TryParse(type, out Notification.NotificationType nType);
        var notification = new Notification
        {
            UserIdToUserType = users,
            Title = title,
            Content = content,
            Type = nType,
            Email = email,
        };

        var eventMessage = _mapper.Map<NotificationEvent>(notification);
        await _publishEndpoint.Publish(eventMessage);
    }
}