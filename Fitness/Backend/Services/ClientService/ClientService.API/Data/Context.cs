using ClientService.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ClientService.API.Data
{
    public class Context:IContext
    {
        public Context(IConfiguration configuration)
        {

            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase("FitDB");

            Clients = database.GetCollection<Client>("Clients");
        }

        public IMongoCollection<Client> Clients { get; }
    }
}
