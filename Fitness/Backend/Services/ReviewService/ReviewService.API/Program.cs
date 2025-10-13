using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReviewService.Common.Extensions;
using System.Text;
using Consul;
using ConsulConfig.Settings;
using ReviewService.API.Publishers;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;
using ReviewService.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));
builder.Services.AddScoped<IReviewPublisher, ReviewPublisher>();

builder.Services.AddControllers();
builder.Services.AddReviewCommonExtensions();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<EventBus.Messages.Events.ReviewEventType, ReviewEventType>().ReverseMap();
    configuration.CreateMap<EventBus.Messages.Events.ReviewEvent, ReviewEvent>().ReverseMap();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
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
