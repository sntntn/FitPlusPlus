using MongoDB.Driver;
using ManagerService.Common.Data;
using ManagerService.Common.Entities;

namespace ManagerService.Common.Repositories;

public class ManagerRepository : IManagerRepository
{
    private readonly IManagerContext _context;
    
    public ManagerRepository(IManagerContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<Manager> GetManagerAsync(string id)
    {
        return await _context.Managers.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateManagerAsync(Manager manager)
    {
        await _context.Managers.InsertOneAsync(manager);
    }

    public async Task<bool> UpdateManagerAsync(Manager manager)
    {
        var updateResult = await _context.Managers.ReplaceOneAsync(m => m.Id == manager.Id, manager);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteManagerAsync(string id)
    {
        var deleteResult = await _context.Managers.DeleteOneAsync(m => m.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<Finance> GetFinanceAsync(string id)
    {
        return await _context.Finances.Find(f => f.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateFinanceAsync(Finance finance)
    {
        await _context.Finances.InsertOneAsync(finance);
    }

    public async Task<bool> UpdateFinanceAsync(Finance finance)
    {
        var updateResult = await _context.Finances.ReplaceOneAsync(f => f.Id == finance.Id, finance);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteFinanceAsync(string id)
    {
        var deleteResult = await _context.Finances.DeleteOneAsync(f => f.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<Trainer> GetTrainerAsync(string id)
    {
        return await _context.Trainers.Find(t => t.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateTrainerAsync(Trainer trainer)
    {
        await _context.Trainers.InsertOneAsync(trainer);
    }

    public async Task<bool> UpdateTrainerAsync(Trainer trainer)
    {
        var updateResult = await _context.Trainers.ReplaceOneAsync(t => t.Id == trainer.Id, trainer);
        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteTrainerAsync(string id)
    {
        var deleteResult = await _context.Trainers.DeleteOneAsync(t => t.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}