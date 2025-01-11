using NotificationService.API.Entities;

namespace NotificationService.API.Repositories;

public interface IRepository
{
    Task<IEnumerable<Notification>> GetNotifications();

    Task<IEnumerable<Notification>> GetNotificationsByUserTypeAndUserId(string UserType, string UserId);

    Task<Notification> GetNotificaitonById(Guid id);

    Task CreateNotification(Notification notification);

    Task<bool> UpdateNotification(Notification notification);

    Task<bool> DeleteNotification(Notification notification);

    Task<bool> DeleteNotificationsByUserTypeAndUserId(string UserType, string UserId);

    Task DeleteAllNotifications();
}