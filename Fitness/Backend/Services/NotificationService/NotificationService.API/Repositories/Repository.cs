using MongoDB.Driver;
using NotificationService.API.Data;
using NotificationService.API.Entities;

namespace NotificationService.API.Repositories;

public class Repository : IRepository
{
    private readonly IContext _context;

    public Repository(IContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Notification>> GetNotifications()
    {
        return await _context.Notifications.Find(n => true).ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId)
    {
        return await _context.Notifications.Find(n => n.UserId == userId).ToListAsync();
    }

    public async Task<Notification> GetNotificationById(string id)
    {
        return await _context.Notifications.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateNotification(Notification notification)
    {
        await _context.Notifications.InsertOneAsync(notification);
    }

    public async Task<bool> UpdateNotification(Notification notification)
    {
        var result = await _context.Notifications.ReplaceOneAsync(n => n.Id == notification.Id, notification);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteNotification(string id)
    {
        var result = await _context.Notifications.DeleteOneAsync(n => n.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> DeleteNotificationsByUserId(string userId)
    {
        var result = await _context.Notifications.DeleteManyAsync(n => n.UserId == userId);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task DeleteAllNotifications()
    {
        await _context.Notifications.DeleteManyAsync(n => true);
    }
}