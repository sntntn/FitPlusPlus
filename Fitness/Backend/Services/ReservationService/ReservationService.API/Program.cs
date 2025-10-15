using Consul;
using ConsulConfig.Settings;
using System.Text;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;
builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));
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

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("secretKey");

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
            ValidAudience = jwtSettings.GetSection("validAudience").Value,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

var app = builder.Build();

app.UseCors("CorsPolicy");

app.Lifetime.ApplicationStarted.Register(() =>
{
    var consulClient = app.Services.GetRequiredService<IConsulClient>();

    var registration = new AgentServiceRegistration
    {
        ID = consulConfig.ServiceId,
        Name = consulConfig.ServiceName,
        Address = consulConfig.ServiceAddress,
        Port = consulConfig.ServicePort
    };
    
    consulClient.Agent.ServiceRegister(registration).Wait();
});

app.Lifetime.ApplicationStopping.Register(() =>
{
    var consulClient = app.Services.GetRequiredService<IConsulClient>();
    consulClient.Agent.ServiceDeregister(consulConfig.ServiceId).Wait();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
