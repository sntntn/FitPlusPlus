using AnalyticsService.Common.Entities;

namespace AnalyticsService.Common.Repositories;

public interface IAnalyticsRepository
{
    Task CreateTraining(Training training);
    Task<double> GetAverageRating(string trainerId);
    Task<int> GetTrainerNumOfTrainings(string trainerId);
    Task<int> GetClientNumOfTraining(string clientId);
}