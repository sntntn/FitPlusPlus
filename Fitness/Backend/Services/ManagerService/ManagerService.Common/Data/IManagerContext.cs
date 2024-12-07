using ManagerService.Common.Entities;
using MongoDB.Driver;

namespace ManagerService.Common.Data;

public interface IManagerContext
{
    IMongoCollection<Manager> Managers { get; }
    IMongoCollection<Trainer> Trainers { get; }
    IMongoCollection<Finance> Finances { get; }
}