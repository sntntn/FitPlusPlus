using ChatService.API.Data;
using ChatService.API.Models;
using ChatService.API.Repositories;
using ChatService.API.Services;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWebSockets();
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/ws") && context.WebSockets.IsWebSocketRequest)
    {
        var trainerId = context.Request.Query["trainerId"];
        var clientId = context.Request.Query["clientId"];

        if (string.IsNullOrEmpty(trainerId) || string.IsNullOrEmpty(clientId))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Missing trainerId or clientId");
            return;
        }

        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var webSocketHandler = app.Services.GetRequiredService<WebSocketHandler>();

        await webSocketHandler.HandleConnection(webSocket, trainerId, clientId);
    }
    else
    {
        await next();
    }
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
