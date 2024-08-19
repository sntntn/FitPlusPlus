using TrainerService.API.Entities;

namespace TrainerService.API.Repositories
{
    public interface ITrainerRepository
    {
        Task<IEnumerable<Trainer>> GetTrainers();

        Task<Trainer> GetTrainer(string id);

        Task<IEnumerable<Trainer>> GetTrainersByTrainingType(string trainingTypeName);

        Task<Trainer> GetTrainerByEmail(string email);

        Task CreateTrainer(Trainer trainer);

        Task<bool> UpdateTrainer(Trainer trainer);

        Task<bool> DeleteTrainer(string id);

        Task<TrainerSchedule> GetTrainerScheduleByTrainerId(string id);
        Task<WeeklySchedule> GetTrainerWeekSchedule(string trainerId, int weekId);
        Task<bool> UpdateTrainerSchedule(TrainerSchedule trainerSchedule);
    }
}
