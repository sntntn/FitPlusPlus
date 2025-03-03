using MongoDB.Driver;
using TrainerService.Common.Entities;

namespace TrainerService.Common.Data
{
    public interface ITrainerContext
    {
        IMongoCollection<Trainer> Trainers { get; }
        IMongoCollection<TrainerSchedule> TrainerSchedules {  get; }
    }
}
