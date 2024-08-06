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

        public async Task<IEnumerable<Trainer>> GetTrainersByTrainingType(string trainingTypeName)
        {
            return await _context.Trainers.Find(p => p.TrainingTypes.Any(pp => pp.Name == trainingTypeName)).ToListAsync();
        }


        public async Task CreateTrainer(Trainer trainer)
        {
            await _context.Trainers.InsertOneAsync(trainer);
        }

        public async Task<bool> UpdateTrainer(Trainer trainer)
        {
            var updateResult = await _context.Trainers.ReplaceOneAsync(p => p.Id == trainer.Id, trainer);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteTrainer(string id)
        {
            var deleteResult = await _context.Trainers.DeleteOneAsync(p => p.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
