using System.Text;
using ClientService.GRPC.Protos;
using Consul;
using ConsulConfig.Settings;
using EventBus.Messages.Constants;
using EventBus.Messages.Events;
using FluentEmail.MailKitSmtp;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NotificationService.API.Data;
using NotificationService.API.Email;
using NotificationService.API.Entities;
using NotificationService.API.EventBusConsumers;
using NotificationService.API.GrpcServices;
using NotificationService.API.Repositories;
using TrainerService.GRPC.Protos;

var builder = WebApplication.CreateBuilder(args);

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;

builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));

// Used for loading .env file
DotNetEnv.Env.Load();

builder.Services.AddScoped<IContext, Context>();
builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddGrpcClient<ClientProtoService.ClientProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:ClientUrl"]!));
builder.Services.AddGrpcClient<TrainerProtoService.TrainerProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:TrainerUrl"]!));
builder.Services.AddScoped<ClientGrpcService>();
builder.Services.AddScoped<TrainerGrpcService>();

var emailSettings = builder.Configuration.GetSection("EmailSettings")!;

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var options = new SmtpClientOptions
{
    Server = emailSettings["SmtpHost"]!,
    Port = int.Parse(emailSettings["SmtpPort"]!),
    User = emailSettings["Username"],
    Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD"),
    UseSsl = bool.Parse(emailSettings["EnableSsl"]!),
    RequiresAuthentication = true
};
builder.Services.AddFluentEmail(emailSettings["FromEmail"]!).AddMailKitSender(options);


builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<NotificationEvent.NotificationType, NotificationReceived.NotificationType>().ReverseMap();
    configuration.CreateMap<NotificationEvent, NotificationReceived>().ReverseMap();
    configuration.CreateMap<NotificationReceived, Notification>().ReverseMap();
    configuration.CreateMap<GetClientResponse, Client>().ReverseMap();
    configuration.CreateMap<GetTrainerResponse, Trainer>().ReverseMap();
});

// EventBus 
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<NotificationConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.NotificationQueue, c =>
        {
            c.ConfigureConsumer<NotificationConsumer>(ctx);
        });
    });
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings.GetValue<string>("secretKey")!;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
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

app.UseCors("CorsPolicy");

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