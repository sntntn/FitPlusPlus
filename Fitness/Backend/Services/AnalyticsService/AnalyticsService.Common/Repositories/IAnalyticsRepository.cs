using AnalyticsService.Common.Entities;

namespace AnalyticsService.Common.Repositories;

public interface IAnalyticsRepository
{
    // Individual Trainings
    Task CreateIndividualTraining(IndividualTraining individualTraining);
    Task<IndividualTraining?> GetIndividualTrainingByReservationId(string reservationId);
    Task<IEnumerable<IndividualTraining>> GetIndividualTrainingsByTrainerId(string trainerId);
    Task<IEnumerable<IndividualTraining>> GetIndividualTrainingsByClientId(string clientId);
    Task<bool> UpdateIndividualTraining(IndividualTraining individualTraining);
    Task<bool> DeleteIndividualTraining(string id);
    
    // Group Trainings
    Task CreateGroupTraining(GroupTraining groupTraining);
    Task<GroupTraining?> GetGroupTrainingByReservationId(string reservationId);
    Task<IEnumerable<GroupTraining>> GetGroupTrainingsByTrainerId(string trainerId);
    Task<IEnumerable<GroupTraining>> GetGroupTrainingsByClientId(string clientId);
    Task<bool> UpdateGroupTraining(GroupTraining groupTraining);
    Task<bool> DeleteGroupTraining(string id);
}