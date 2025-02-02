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
        var notification = _mapper.Map<Notification>(context.Message);
        await _repository.CreateNotification(notification);
        
        if (notification.Email)
        {
            var clients = await _clientGrpcService.GetClients(notification.UserIdToUserType.Keys);
            var clientsList = _mapper.Map<IEnumerable<Client>>(clients.Clients);

            var trainers = await _trainerGrpcService.GetTrainers(notification.UserIdToUserType.Keys);
            var trainersList = _mapper.Map<IEnumerable<Trainer>>(trainers.Trainers);
            
            var subject = "[FitPlusPlus Gym] " + notification.Title;
            var body = "You have a new notification from your FitPlusPlus Gym Account:\n\n" + notification.Content +
                          "\n\nTime: " + notification.CreationDate;
            
            foreach (var client in clientsList)
            {
                await _emailService.SendEmailAsync(
                    to: client.Email,
                    subject: subject,
                    body: body
                );
            }
            
            foreach (var trainer in trainersList)
            {
                await _emailService.SendEmailAsync(
                    to: trainer.Email,
                    subject: subject,
                    body: body
                );
            }
        }
    }
}