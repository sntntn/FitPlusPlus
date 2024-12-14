using videoTrainingService.API.Entities;

namespace videoTrainingService.API.Repositories
{
    public interface ITrainingRepository
    {
        Task<IEnumerable<Exercise>> GetExercises(string trainerId);
        Task<Exercise> GetExercise(string name, string trainerId);
        Task CreateExercise(Exercise exercise);
        Task<bool> UpdateExercise(Exercise exercise);
        Task<bool> DeleteExercise(string id);
        Task<IEnumerable<Training>> GetTrainingsForClient(string clientId);
        Task<IEnumerable<Training>> GetTrainingsForTrainer(string trainerId);
        Task<Training> GetTraining(string id);
        Task CreateTraining(Training training);
        Task<bool> UpdateTraining(Training training);
        Task<bool> DeleteTraining(string id);
        Task<IEnumerable<TrainingExercise>> GetTrainingExercises(string trainingId);
        Task CreateTrainingExercise(TrainingExercise trainingExercise);
        Task<bool> UpdateTrainingExercise(TrainingExercise trainingExercise);
        Task<bool> DeleteTrainingExercise(string trainingId, string id);
    }
}