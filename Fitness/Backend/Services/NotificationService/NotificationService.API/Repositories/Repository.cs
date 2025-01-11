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

    public async Task<IEnumerable<Notification>> GetNotificationsByUserTypeAndUserId(string UserType, string UserId)
    {
        return await _context.Notifications.Find(p => p.UserType == UserType && p.UserType == UserId).ToListAsync();
    }

    public async Task<Notification> GetNotificationById(Guid id)
    {
        return await _context.Notifications.Find(p => p.Id == id).FirstOrDefaultAsync();
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

    public async Task<bool> DeleteNotification(Guid id)
    {
        var result = await _context.Notifications.DeleteOneAsync(n => n.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<bool> DeleteNotificationsByUserTypeAndUserId(string UserType, string UserId)
    {
        var result = await _context.Notifications.DeleteManyAsync(n => n.UserType == UserType && n.UserId == UserType);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task DeleteAllNotifications()
    {
        await _context.Notifications.DeleteManyAsync(n => true);
    }
}