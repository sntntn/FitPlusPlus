using ClientService.Common.Entities;

namespace ClientService.Common.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client> GetClientById(string id);
        Task<IEnumerable<Client>> GetClientsByIds(IEnumerable<string> ids);
        Task<IEnumerable<Client>> GetClientsByName(string name);
        Task<IEnumerable<Client>> GetClientsBySurname(string surname);
        Task<Client> GetClientByEmail(string email);
        Task CreateClient(Client client);
        Task<bool> UpdateClient(Client client);
        Task<bool> DeleteClient(string id);
        Task DeleteAllClients();
    }
}
