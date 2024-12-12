using MongoDB.Driver;
using videoTrainingService.API.Entities;

namespace videoTrainingService.API.Data;

public class TrainingContext : ITrainingContext
{
    public TrainingContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase("TrainingDB");

        Exercise = database.GetCollection<Exercise>("Exercise");
        Training = database.GetCollection<Training>("Training");
        TrainingExercise = database.GetCollection<TrainingExercise>("TrainingExercise");
    }

    public IMongoCollection<Exercise> Exercise { get; }
    public IMongoCollection<Training> Training { get; }
    public IMongoCollection<TrainingExercise> TrainingExercise { get; }
}