using ManagerService.Common.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ManagerService.Common.Data;

public class ManagerContext : IManagerContext
{
    
    public ManagerContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase("ManagerDB");

        Managers = database.GetCollection<Manager>("Managers");
        Finances = database.GetCollection<Finance>("Finances");
        Trainers = database.GetCollection<Trainer>("Trainers");
    }
    
    public IMongoCollection<Manager> Managers { get; }
    public IMongoCollection<Trainer> Trainers { get; }
    public IMongoCollection<Finance> Finances { get; }
}