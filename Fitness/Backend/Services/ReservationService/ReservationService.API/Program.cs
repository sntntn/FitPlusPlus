using EventBus.Messages.Events;
using MassTransit;
using ReservationService.API.Data;
using ReservationService.API.Entities;
using ReservationService.API.Publishers;
using ReservationService.API.Repository;
using ReservationService.API.Services;
using IndividualReservationEvent = ReservationService.API.Entities.IndividualReservationEvent;
using IndividualReservationEventType = ReservationService.API.Entities.IndividualReservationEventType;
using GroupReservationEvent = ReservationService.API.Entities.GroupReservationEvent;
using GroupReservationEventType = ReservationService.API.Entities.GroupReservationEventType;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContext, Context>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<INotificationPublisher, NotificationPublisher>();
builder.Services.AddScoped<IGroupReservationPublisher, GroupReservationPublisher>();
builder.Services.AddScoped<IIndividualReservationPublisher, IndividualReservationPublisher>();
builder.Services.AddScoped<IReservationService, ReservationService.API.Services.ReservationService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<NotificationEvent.NotificationType, Notification.NotificationType>().ReverseMap();
    configuration.CreateMap<NotificationEvent, Notification>().ReverseMap();
    configuration.CreateMap<EventBus.Messages.Events.IndividualReservationEventType, IndividualReservationEventType>().ReverseMap();
    configuration.CreateMap<EventBus.Messages.Events.IndividualReservationEvent, IndividualReservationEvent>().ReverseMap();
    configuration.CreateMap<EventBus.Messages.Events.GroupReservationEventType, GroupReservationEventType>().ReverseMap();
    configuration.CreateMap<EventBus.Messages.Events.GroupReservationEvent, GroupReservationEvent>().ReverseMap();
});

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
