using AutoMapper;
using MassTransit;
using EventBus.Messages.Events;
using NotificationService.API.Entities;
using NotificationService.API.Repositories;

namespace NotificationService.API.EventBusConsumers;

public class NotificationConsumer : IConsumer<NotificationEvent>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public NotificationConsumer(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public async Task Consume(ConsumeContext<NotificationEvent> context)
    {
        var notification = _mapper.Map<Notification>(context.Message);
        await _repository.CreateNotification(notification);
        
        if (notification.Email)
        {
            // TODO("Send email");
        }

        if (notification.Push)
        {
            // TODO("Send push notification");
        }
    }
}