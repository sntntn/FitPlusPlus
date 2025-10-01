using EventBus.Messages.Constants;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReviewService.GRPC.Protos;
using System.Text;
using Consul;
using ConsulConfig.Settings;
using TrainerService.API.Data;
using TrainerService.API.Entities;
using TrainerService.API.EventBusConsumers;
using TrainerService.API.GrpcServices;
using TrainerService.API.Repositories;

var builder = WebApplication.CreateBuilder(args);

var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;

builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));

// Add services to the container.

builder.Services.AddScoped<ITrainerContext, TrainerContext>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();

builder.Services.AddGrpcClient<ReviewProtoService.ReviewProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:ReviewUrl"]));
builder.Services.AddScoped<ReviewGrpcService>();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<GetReviewsResponse, ReviewType>().ReverseMap();
    configuration.CreateMap<GetReviewsResponse.Types.ReviewReply, ReviewType>().ReverseMap();
    configuration.CreateMap<BookTrainingInformation, BookTrainingEvent>().ReverseMap();
    configuration.CreateMap<TrainerCancellingTrainingEvent, CancelTrainingInformation>().ReverseMap();
});

//EventBus
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BookTrainingConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.BookTrainingQueue,c =>
        {
            c.ConfigureConsumer<BookTrainingConsumer>(ctx);
        });
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddControllers();
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

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();
