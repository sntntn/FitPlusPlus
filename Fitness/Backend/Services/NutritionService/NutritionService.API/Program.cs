using Consul;
using ConsulConfig.Settings;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using NutritionService.API.Repositories;


var builder = WebApplication.CreateBuilder(args);
var consulConfig = builder.Configuration.GetSection("ConsulConfig").Get<ConsulConfiguration>()!;
builder.Services.AddSingleton(consulConfig);
builder.Services.AddSingleton<IConsulClient, ConsulClient>(provider => new ConsulClient(config =>
{
    config.Address = new Uri(consulConfig.Address);
}));

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var config = builder.Configuration.GetSection("DatabaseSettings");
    var client = new MongoClient(config["ConnectionString"]);
    return client.GetDatabase(config["DatabaseName"]);
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(o =>
{
    o.AddPolicy("CorsPolicy", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSingleton<IFoodRepository, FoodRepository>();

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


