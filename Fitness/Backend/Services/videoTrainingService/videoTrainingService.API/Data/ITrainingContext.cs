using MongoDB.Driver;
using videoTrainingService.API.Entities;

namespace videoTrainingService.API.Data;

public interface ITrainingContext
{
    IMongoCollection<Exercise> Exercise { get; }
    
    IMongoCollection<Training> Training { get; }
    
    IMongoCollection<TrainingExercise> TrainingExercise { get; }
}