using MongoDB.Driver;
using videoTrainingService.API.Data;
using videoTrainingService.API.Entities;

namespace videoTrainingService.API.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private readonly ITrainingContext _context;

        public TrainingRepository(ITrainingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Exercise>> GetExercises(string trainerId)
        {
            return await _context.Exercises.Find(p => p.TrainerId == trainerId).ToListAsync();
        }

        public async Task<Exercise> GetExercise(string id)
        {
            return await _context.Exercises.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateExercise(Exercise exercise)
        {
            await _context.Exercises.InsertOneAsync(exercise);
        }

        public async Task<bool> UpdateExercise(Exercise exercise)
        {
            var res = await _context.Exercises.ReplaceOneAsync(p => p.Id == exercise.Id, exercise);
            return res.IsAcknowledged && res.ModifiedCount > 0;
        }

        public async Task<bool> DeleteExercise(string id)
        {
            var res = await _context.Exercises.DeleteOneAsync(p => p.Id == id);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }

        public async Task<IEnumerable<Training>> GetTrainingsForClient(string clientId)
        {
            return await _context.Trainings.Find(p => p.ClientId == clientId).ToListAsync();
        }

        public async Task<IEnumerable<Training>> GetTrainingsForTrainer(string trainerId)
        {
            return await _context.Trainings.Find(p => p.ClientId == trainerId).ToListAsync();
        }

        public async Task<Training> GetTraining(string id)
        {
            return await _context.Trainings.Find(p => p.TrainingId == id).FirstOrDefaultAsync();
        }

        public async Task CreateTraining(Training training)
        {
            await _context.Trainings.InsertOneAsync(training);
        }

        public async Task<bool> UpdateTraining(Training training)
        {
            var res = await _context.Trainings.ReplaceOneAsync(p => p.TrainingId == training.TrainingId, training);
            return res.IsAcknowledged && res.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTraining(string id)
        {
            var res = await _context.Trainings.DeleteOneAsync(p => p.TrainingId == id);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }

        public async Task<IEnumerable<TrainingExercise>> GetTrainingExercises(string trainingId)
        {
            return await _context.TrainingExercises.Find(p => p.TrainingId == trainingId).ToListAsync();
        }

        public async Task<TrainingExercise> GetTrainingExercise(string id)
        {
            return await _context.TrainingExercises.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task CreateTrainingExercise(TrainingExercise trainingExercise)
        {
            await _context.TrainingExercises.InsertOneAsync(trainingExercise);
        }

        public async Task<bool> UpdateTrainingExercise(TrainingExercise trainingExercise)
        {
            var res = await _context.TrainingExercises.ReplaceOneAsync(p => p.TrainingId == trainingExercise.TrainingId && 
                p.ExerciseId == trainingExercise.ExerciseId, trainingExercise);
            return res.IsAcknowledged && res.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTrainingExercise(string id)
        {
            var res = await _context.TrainingExercises.DeleteOneAsync(p => p.Id == id);
            return res.IsAcknowledged && res.DeletedCount > 0;
        }
    }
}