using MongoDB.Driver;
using NutritionService.API.Repositories;


var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();


