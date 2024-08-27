using MongoDB.Driver;
using TrainerService.API.Entities;

namespace TrainerService.API.Data
{
    public class TrainerContext : ITrainerContext
    {
        public TrainerContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase("TrainerDB");

            Trainers = database.GetCollection<Trainer>("Trainers");

            TrainerSchedules = database.GetCollection<TrainerSchedule>("TrainerSchedules");
        }

        public IMongoCollection<Trainer> Trainers { get; }
        public IMongoCollection<TrainerSchedule> TrainerSchedules { get; }
    }
}
