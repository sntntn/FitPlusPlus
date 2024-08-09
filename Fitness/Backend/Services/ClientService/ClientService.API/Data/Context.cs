using ClientService.API.Entities;
using MongoDB.Driver;

namespace ClientService.API.Data
{
    public class Context:IContext
    {
        public Context()
        {

            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("FitDB");

            Clients = database.GetCollection<Client>("Clients");
        }

        public IMongoCollection<Client> Clients { get; }
    }
}
