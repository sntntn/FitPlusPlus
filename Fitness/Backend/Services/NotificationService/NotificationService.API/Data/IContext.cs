using MongoDB.Driver;
using NotificationService.API.Entities;

namespace NotificationService.API.Data;

public interface IContext
{
    IMongoCollection<Notification> Notifications { get; }
}