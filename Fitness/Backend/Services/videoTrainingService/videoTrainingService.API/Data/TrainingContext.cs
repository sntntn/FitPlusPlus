using MongoDB.Driver;
using videoTrainingService.API.Entities;

namespace videoTrainingService.API.Data
{
    public class TrainingContext : ITrainingContext
    {
        public TrainingContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase("TrainingDB");

            Exercises = database.GetCollection<Exercise>("Exercises");
            Trainings = database.GetCollection<Training>("Trainings");
            TrainingExercises = database.GetCollection<TrainingExercise>("TrainingExercises");
        }

        public IMongoCollection<Exercise> Exercises { get; }
        public IMongoCollection<Training> Trainings { get; }
        public IMongoCollection<TrainingExercise> TrainingExercises { get; }
    }
}