using System.Reflection;
using System.Text;
using AnalyticsService.API.EventBusConsumers;
using AnalyticsService.API.GrpcServices;
using AnalyticsService.Common.Extensions;
using EventBus.Messages.Constants;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ReviewService.GRPC.Protos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddAnalyticsCommonExtensions();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    config.AddConsumer<IndividualReservationConsumer>();
    config.AddConsumer<GroupReservationConsumer>();
    config.AddConsumer<ReviewConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration["EventBusSettings:HostAddress"]);
        cfg.ReceiveEndpoint(EventBusConstants.IndividualReservationQueue, c =>
        {
            c.ConfigureConsumer<IndividualReservationConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(EventBusConstants.GroupReservationQueue, c =>
        {
            c.ConfigureConsumer<GroupReservationConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(EventBusConstants.ReviewQueue, c =>
        {
            c.ConfigureConsumer<ReviewConsumer>(ctx);
        });
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseRouting();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
