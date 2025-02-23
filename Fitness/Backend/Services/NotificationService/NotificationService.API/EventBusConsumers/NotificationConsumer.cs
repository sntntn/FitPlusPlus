using AutoMapper;
using MassTransit;
using EventBus.Messages.Events;
using NotificationService.API.Email;
using NotificationService.API.Entities;
using NotificationService.API.GrpcServices;
using NotificationService.API.Repositories;

namespace NotificationService.API.EventBusConsumers;

public class NotificationConsumer : IConsumer<NotificationEvent>
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ClientGrpcService _clientGrpcService;
    private readonly TrainerGrpcService _trainerGrpcService;

    public NotificationConsumer(IRepository repository, IMapper mapper, IEmailService emailService, ClientGrpcService clientGrpcService, TrainerGrpcService trainerGrpcService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _clientGrpcService = clientGrpcService ?? throw new ArgumentNullException(nameof(clientGrpcService));
        _trainerGrpcService = trainerGrpcService ?? throw new ArgumentNullException(nameof(trainerGrpcService));
    }
    
    public async Task Consume(ConsumeContext<NotificationEvent> context)
    {
        var notification = _mapper.Map<NotificationReceived>(context.Message);
        await _repository.CreateNotification(_mapper.Map<Notification>(notification));
        
        if (notification.Email)
        {
            var client = await _clientGrpcService.GetClient(notification.UserId);
            var clientInfo = _mapper.Map<Client>(client);

            // var trainer = await _trainerGrpcService.GetTrainers(notification.UserId);
            // var trainerInfo = _mapper.Map<Trainer>(trainer);
            
            var subject = "[FitPlusPlus Gym] " + notification.Title;
            var body = "You have a new notification from your FitPlusPlus Gym Account:\n\n" + notification.Content +
                          "\n\nTime: " + notification.CreationDate;
            
            if (clientInfo != null)
            {
                await _emailService.SendEmailAsync(clientInfo.Email, subject, body);
            }

            // if (trainerInfo != null)
            // { 
            //     Console.WriteLine(trainerInfo.Email);
            //     await _emailService.SendEmailAsync(trainerInfo.Email, subject, body);
            // }
        }
    }
}