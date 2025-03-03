using NotificationService.API.Entities;

namespace NotificationService.API.Repositories;

public interface IRepository
{
    Task<IEnumerable<Notification>> GetNotifications();

    Task<IEnumerable<Notification>> GetNotificationsByUserId(string userId);

    Task<Notification> GetNotificationById(string id);

    Task CreateNotification(Notification notification);

    Task<bool> UpdateNotification(Notification notification);

    Task<bool> DeleteNotification(string id);

    Task<bool> DeleteNotificationsByUserId(string userId);

    Task DeleteAllNotifications();
}