using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using ClientService.GRPC.Protos;
using EventBus.Messages.Constants;
using EventBus.Messages.Events;
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

builder.Services.AddFluentEmail(emailSettings["FromEmail"]!)
    .AddSmtpSender(new SmtpClient
    {
        Host = emailSettings["SmtpHost"]!,
        Port = int.Parse(emailSettings["SmtpPort"]!),
        Credentials = new NetworkCredential(emailSettings["Username"]!, Environment.GetEnvironmentVariable("EMAIL_PASSWORD")!),
        EnableSsl = bool.Parse(emailSettings["EnableSsl"]!)
    });

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<NotificationEvent.NotificationType, Notification.NotificationType>().ReverseMap();
    configuration.CreateMap<NotificationEvent, Notification>().ReverseMap();
    configuration.CreateMap<GetClientsResponse.Types.ClientReply, Client>().ReverseMap();
    configuration.CreateMap<GetTrainersResponse.Types.TrainerReply, Trainer>().ReverseMap();
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