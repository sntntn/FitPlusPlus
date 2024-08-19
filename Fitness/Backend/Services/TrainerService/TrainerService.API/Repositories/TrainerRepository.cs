using MongoDB.Bson;
using MongoDB.Driver;
using System.Net.Http.Headers;
using TrainerService.API.Data;
using TrainerService.API.Entities;

namespace TrainerService.API.Repositories
{
    public class TrainerRepository : ITrainerRepository
    {
        private readonly ITrainerContext _context;

        public TrainerRepository(ITrainerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Trainer>> GetTrainers()
        {
            return await _context.Trainers.Find(p => true).ToListAsync();
        }

        public async Task<Trainer> GetTrainer(string id)
        {
            return await _context.Trainers.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Trainer> GetTrainerByEmail(string email)
        {
            return await _context.Trainers.Find(p => p.ContactEmail == email).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Trainer>> GetTrainersByTrainingType(string trainingTypeName)
        {
            return await _context.Trainers.Find(p => p.TrainingTypes.Any(pp => pp.Name == trainingTypeName)).ToListAsync();
        }


        public async Task CreateTrainer(Trainer trainer)
        {
            await _context.Trainers.InsertOneAsync(trainer);
            var trainerSchedule = new TrainerSchedule(trainer.Id);
            await _context.TrainerSchedules.InsertOneAsync(trainerSchedule);
        }

        public async Task<bool> UpdateTrainer(Trainer trainer)
        {
            var updateResult = await _context.Trainers.ReplaceOneAsync(p => p.Id == trainer.Id, trainer);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTrainer(string id)
        {
            var deleteResult = await _context.Trainers.DeleteOneAsync(p => p.Id == id);
            var deleteScheduleResult = await _context.TrainerSchedules.DeleteOneAsync(ts => ts.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0
                && deleteScheduleResult.IsAcknowledged && deleteResult.DeletedCount>0;
        }

        public async Task<TrainerSchedule> GetTrainerScheduleByTrainerId(string id)
        {
            return await _context.TrainerSchedules.Find(s => s.TrainerId == id).FirstOrDefaultAsync();
        }
        public async Task<WeeklySchedule> GetTrainerWeekSchedule(string trainerId, int weekId)
        {
            var trainerSchedule = await GetTrainerScheduleByTrainerId(trainerId);
            return trainerSchedule?.WeeklySchedules.FirstOrDefault(ws => ws.WeekId == weekId);
        }
        public async Task<bool> UpdateTrainerSchedule(TrainerSchedule trainerSchedule)
        {
            var result = await _context.TrainerSchedules.ReplaceOneAsync(cs => cs.TrainerId == trainerSchedule.TrainerId, trainerSchedule, new ReplaceOptions { IsUpsert = true });
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
    }
}
