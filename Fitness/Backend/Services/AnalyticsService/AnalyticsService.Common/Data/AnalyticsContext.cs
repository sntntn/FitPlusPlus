using AnalyticsService.Common.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AnalyticsService.Common.Data;

public class AnalyticsContext : IAnalyticsContext
{
    public AnalyticsContext(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = mongoClient.GetDatabase("AnalyticsDB");
        Trainings = database.GetCollection<Training>("Trainings");
        SeedData(Trainings);
    }
    
    public IMongoCollection<Training> Trainings { get; set; }

    private static void SeedData(IMongoCollection<Training> trainings)
    {
        if (!trainings.Find(t => true).Any())
        {
            trainings.InsertOneAsync(
                new Training()
                {
                    ClientId = "1234567890abcdef12345678",
                    TrainerId = "1234567890abcdef12345678",
                    TrainingDate = new DateTime(2020, 01, 01),
                    TrainerRating = 5,
                    Status = TrainingStatus.HELD
                }
            );
            trainings.InsertOneAsync(
                new Training()
                {
                    ClientId = "5F6D3326597F61D77AD99969",
                    TrainerId = "1234567890abcdef12345678",
                    TrainingDate = new DateTime(2021, 01, 01),
                    ClientRating = 7,
                    Status = TrainingStatus.CANCELLED,
                    ClientComment = "Unpleasant experience"
                }
            );
        }
    }
}