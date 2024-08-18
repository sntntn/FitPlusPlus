using ClientService.API.Entities;
using MongoDB.Driver;

namespace ClientService.API.Data
{
    public interface IContext
    {
        IMongoCollection<Client> Clients { get; }
        IMongoCollection<ClientSchedule> ClientSchedules { get; }
    }
}
