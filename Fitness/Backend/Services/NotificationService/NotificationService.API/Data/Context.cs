using MongoDB.Driver;
using NotificationService.API.Entities;

namespace NotificationService.API.Data;

public class Context : IContext
{
    public Context(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = client.GetDatabase("Notifications");

        Notifications = database.GetCollection<Notification>("Notifications");
    }
    
    public IMongoCollection<Notification> Notifications { get;  }

}