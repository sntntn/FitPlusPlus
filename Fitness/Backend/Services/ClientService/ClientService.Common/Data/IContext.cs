using ClientService.Common.Entities;
using MongoDB.Driver;

namespace ClientService.Common.Data
{
    public interface IContext
    {
        IMongoCollection<Client> Clients { get; }
    }
}
