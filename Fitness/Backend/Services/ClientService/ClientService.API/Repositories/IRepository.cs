using ClientService.API.Entities;

namespace ClientService.API.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client> GetClientById(string id);
        Task<IEnumerable<Client>> GetClientsByName(string name);
        Task<IEnumerable<Client>> GetClientsBySurname(string surname);
        Task CreateClient(Client client);
        Task<bool> UpdateClient(Client client);
        Task<bool> DeleteClient(string id);
        Task DeleteAllClients();
    }
}
