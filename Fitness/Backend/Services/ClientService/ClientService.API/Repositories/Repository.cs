using Amazon.Runtime.Internal;
using ClientService.API.Data;
using ClientService.API.Entities;
using MongoDB.Driver;

namespace ClientService.API.Repositories
{
    public class Repository : IRepository
    {
        private readonly IContext _context;
        public Repository(IContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _context.Clients.Find(p => true).ToListAsync();
        }
        public async Task<Client> GetClientById(string id)
        {
            return await _context.Clients.Find(p => p.Id == id).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<Client>> GetClientsByName(string name)
        {
            return await _context.Clients.Find(p => p.Name == name).ToListAsync();
        }
        public async Task<IEnumerable<Client>> GetClientsBySurname(string surname)
        {
            return await _context.Clients.Find(p => p.Surname == surname).ToListAsync();
        }
        public async Task CreateClient(Client client)
        {
            await _context.Clients.InsertOneAsync(client);
        }
        public async Task<bool> UpdateClient(Client client)
        {
            var result = await _context.Clients.ReplaceOneAsync(p => p.Id == client.Id, client);
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        public async Task<bool> DeleteClient(string id)
        {
            var resultClient = await _context.Clients.DeleteOneAsync(p => p.Id == id);
            return resultClient.IsAcknowledged && resultClient.DeletedCount > 0;
        }
        public async Task DeleteAllClients()
        {
            await _context.Clients.DeleteManyAsync(p => true);
        }
    }
}
