using ChatService.API.Data;
using ChatService.API.Middleware;
using ChatService.API.Models;
using ChatService.API.Repositories;
using ChatService.API.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Consul;
using ConsulConfig.Settings;


var builder = WebApplication.CreateBuilder(args);

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;

builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));

// Register MongoDB client
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    var settings = serviceProvider.GetRequiredService<IOptions<MongoDBSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Register Data
builder.Services.AddScoped<IContext, Context>();

// Register ChatRepository
builder.Services.AddScoped<IChatRepository, ChatRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddSingleton<WebSocketHandler>();

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



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.MapControllers();

app.Run();
