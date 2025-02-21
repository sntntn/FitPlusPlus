using AnalyticsService.Common.Entities;

namespace AnalyticsService.Common.Repositories;

public interface IAnalyticsRepository
{
    Task CreateTraining(Training training);
    Task<bool> DeleteTraining(string id);
    Task<double> GetTrainerAverageTrainingRating(string trainerId);
    Task<int> GetTrainerNumOfTrainings(string trainerId);
    Task<int> GetClientNumOfTrainings(string clientId);
    Task<int> GetClientNumOfHeldTrainings(string clientId);
    Task<int> GetClientNumOfCancelledTrainings(string clientId);
    Task<IEnumerable<string>> GetTrainerClientIds(string trainerId);
}