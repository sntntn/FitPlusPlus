using Consul;
using ConsulConfig.Settings;
using ReviewService.Common.DTOs;
using ReviewService.Common.Extensions;
using ReviewService.GRPC.Protos;
using ReviewService.GRPC.Services;

var builder = WebApplication.CreateBuilder(args);

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;

builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));


// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddReviewCommonExtensions();
builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<ReviewDTO, GetReviewsResponse.Types.ReviewReply>().ReverseMap();
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

// Configure the HTTP request pipeline.
app.MapGrpcService<ReviewServiceGrpc>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
