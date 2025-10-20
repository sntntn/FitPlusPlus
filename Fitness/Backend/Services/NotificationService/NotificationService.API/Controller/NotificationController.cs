using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Entities;
using NotificationService.API.Repositories;

namespace NotificationService.API.Controller;

/// <summary>
/// Provides REST API endpoints for fetching and modifying notification data.
/// </summary>
/// <remarks>
/// This controller exposes operations for fetching and modyfing notification data.
///
/// All routes are secured and require authorization with roles <c>Admin</c>, <c>Trainer</c>, or <c>Client</c>.
/// </remarks>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IRepository _repository;

    public NotificationController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// API for fetching all notifications.
    /// </summary>
    /// <returns><c>IEnumerable</c> of all notifications.</returns>
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
    {
        var notifications = await _repository.GetNotifications();
        return Ok(notifications);
    }

    /// <summary>
    /// API for fetching all notifications associated with a given user.
    /// </summary>
    /// <param name="userId">Unique identifier of the desired user</param>
    /// <returns><c>IEnumerable</c> of notifications for the given user.</returns>
    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(string userId)
    {
        var notifications = await _repository.GetNotificationsByUserId(userId);
        return Ok(notifications);
    }

    /// <summary>
    /// API for fetching a notification with the given identifier.
    /// </summary>
    /// <param name="id">Unique identifier of the notification</param>
    /// <returns>Notification with the given identifier.</returns>
    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<ActionResult<Notification>> GetNotificationById(string id)
    {
        var notification = await _repository.GetNotificationById(id);
        return Ok(notification);
    }

    /// <summary>
    /// API for updating a notification.
    /// </summary>
    /// <param name="notification">Notification object containing updated values.</param>
    /// <returns>Updated notification object.</returns>
    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpPut]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNotification([FromBody] Notification notification)
    {
        return Ok(await _repository.UpdateNotification(notification));
    }
    
    /// <summary>
    /// API for marking a notification (with the given identifier) as read.
    /// </summary>
    /// <param name="id">Unique identifier of the notification to be marked as read.</param>
    /// <returns>200 OK if successful, 404 NotFound if notification does not exist.</returns>
    [Authorize(Roles = "Trainer, Client")]
    [HttpPut("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        var success = await _repository.MarkNotificationAsRead(id);

        if (!success)
            return NotFound();

        return Ok(new { Message = "Notification marked as read" });
    }

    /// <summary>
    /// API for deleting all notifications.
    /// </summary>
    /// <returns>200 OK when all notifications are deleted.</returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotifications()
    {
        await _repository.DeleteAllNotifications();
        return Ok();
    }

    /// <summary>
    /// API for deleting all notifications for a given user.
    /// </summary>
    /// <param name="userId">Unique identifier of the user whose notifications should be deleted.</param>
    /// <returns>200 OK when notifications are deleted.</returns>
    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpDelete("/user/{userId}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotificationsByUserTypeAndUserId(string userId)
    {
        return Ok(await _repository.DeleteNotificationsByUserId(userId));
    }

    /// <summary>
    /// API for deleting a notification with the given identifier.
    /// </summary>
    /// <param name="id">Unique identifier of the notification.</param>
    /// <returns>200 OK when the notification is deleted.</returns>
    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotification(string id)
    {
        return Ok( await _repository.DeleteNotification(id));
    }
}