using ManagerService.Common.Entities;

namespace ManagerService.Common.Repositories;

public interface IManagerRepository
{
    Task<Manager> GetManagerAsync(string id);
    Task CreateManagerAsync(Manager manager);
    Task<bool> UpdateManagerAsync(Manager manager);
    Task<bool> DeleteManagerAsync(string id);
    
    Task<Finance> GetFinanceAsync(string id);
    Task CreateFinanceAsync(Finance finance);
    Task<bool> UpdateFinanceAsync(Finance finance);
    Task<bool> DeleteFinanceAsync(string id);
    
    Task<Trainer> GetTrainerAsync(string id);
    Task CreateTrainerAsync(Trainer trainer);
    Task<bool> UpdateTrainerAsync(Trainer trainer);
    Task<bool> DeleteTrainerAsync(string id);


}