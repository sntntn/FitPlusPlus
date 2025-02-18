using EventBus.Messages.Events;
using MassTransit;
using ReservationService.API.Data;
using ReservationService.API.Entities;
using ReservationService.API.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IContext, Context>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
});

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((_, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
    });
});
var app = builder.Build();

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
