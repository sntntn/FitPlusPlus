using System.Reflection;
using AnalyticsService.API.EventBusConsumers;
using AnalyticsService.API.GrpcServices;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Extensions;
using EventBus.Messages.Constants;
using MassTransit;
using ReviewService.GRPC.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAnalyticsCommonExtensions();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddGrpcClient<ReviewProtoService.ReviewProtoServiceClient>(
    options => options.Address = new Uri(builder.Configuration["GrpcSettings:ReviewUrl"]));
builder.Services.AddScoped<ReviewGrpcService>();

// cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

//EventBus
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<TrainingHeldConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.TrainingHeldQueue, c =>
        {
            c.ConfigureConsumer<TrainingHeldConsumer>(ctx);
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
