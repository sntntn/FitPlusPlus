using MongoDB.Driver;
using videoTrainingService.API.Entities;

namespace videoTrainingService.API.Data
{
    public interface ITrainingContext
    {
        IMongoCollection<Exercise> Exercises { get; }
        IMongoCollection<Training> Trainings { get; }
        IMongoCollection<TrainingExercise> TrainingExercises { get; }
    }
}