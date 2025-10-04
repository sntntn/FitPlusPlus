using System.Collections;
using TrainerService.Common.Entities;

namespace TrainerService.Common.Repositories
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<Trainer>> GetTrainers();
        Task<IEnumerable<Trainer>> GetTrainersByIds(IEnumerable<string> ids);
        Task<Trainer> GetTrainer(string id);
        Task<IEnumerable<Trainer>> GetTrainersByTrainingType(string trainingTypeName);
        Task<Trainer> GetTrainerByEmail(string email);
        Task<decimal> GetPrice(string trainerId, string trainingType);
        Task CreateTrainer(Trainer trainer);
        Task<bool> UpdateTrainer(Trainer trainer);
        Task<bool> DeleteTrainer(string id);
    }
}
