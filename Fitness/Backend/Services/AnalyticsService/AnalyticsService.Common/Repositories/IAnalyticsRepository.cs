namespace AnalyticsService.Common.Repositories;

public interface IAnalyticsRepository
{
    Task<double> GetAverageRating(string trainerId);
    Task<int> GetTrainerNumOfTrainings(string trainerId);
    Task<int> GetClientNumOfTraining(string clientId);
}