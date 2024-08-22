using ClientService.API.Entities;

namespace ClientService.API.Repositories
{
    public interface IRepository
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client> GetClientById(string id);
        Task<IEnumerable<Client>> GetClientsByName(string name);
        Task<IEnumerable<Client>> GetClientsBySurname(string surname);
        Task<Client> GetClientByEmail(string email);
        Task CreateClient(Client client);
        Task<bool> UpdateClient(Client client);
        Task<bool> DeleteClient(string id);
        Task DeleteAllClients();
        Task<ClientSchedule> GetClientScheduleByClientId(string id);
        Task<WeeklySchedule> GetClientWeekSchedule(string clientId, int weekId);
        Task<bool> UpdateClientSchedule(ClientSchedule clientSchedule);
        Task<bool> BookTraining(BookTrainingInformation bti);
    }
}
