using ClientService.Common.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ClientService.Common.Data
{
    public class Context:IContext
    {
        public Context(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase("ClientsAndSchedules");

            Clients = database.GetCollection<Client>("Clients");
            ClientSchedules = database.GetCollection<ClientSchedule>("ClientSchedules");
        }

        public IMongoCollection<Client> Clients { get; }
        public IMongoCollection<ClientSchedule> ClientSchedules { get; }
    }
}
