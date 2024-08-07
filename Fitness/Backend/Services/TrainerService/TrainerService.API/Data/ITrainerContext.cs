using MongoDB.Driver;
using TrainerService.API.Entities;

namespace TrainerService.API.Data
{
    public interface ITrainerContext
    {
        IMongoCollection<Trainer> Trainers { get; }
    }
}
