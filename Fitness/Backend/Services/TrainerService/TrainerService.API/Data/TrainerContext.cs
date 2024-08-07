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
            TrainerContextSeed.SeedData(Trainers);
        }

        public IMongoCollection<Trainer> Trainers { get; }
    }
}
