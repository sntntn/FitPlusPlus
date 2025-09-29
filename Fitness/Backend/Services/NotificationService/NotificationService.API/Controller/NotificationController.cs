using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Entities;
using NotificationService.API.Repositories;

namespace NotificationService.API.Controller;

//[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IRepository _repository;

    public NotificationController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
    {
        var notifications = await _repository.GetNotifications();
        return Ok(notifications);
    }

    //[Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserId(string userId)
    {
        var notifications = await _repository.GetNotificationsByUserId(userId);
        return Ok(notifications);
    }

    //[Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<ActionResult<Notification>> GetNotificationById(string id)
    {
        var notification = await _repository.GetNotificationById(id);
        return Ok(notification);
    }

    //[Authorize(Roles = "Admin, Trainer, Client")]
    [HttpPut]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNotification([FromBody] Notification notification)
    {
        return Ok(await _repository.UpdateNotification(notification));
    }
    
    // PUT api/notifications/{id}/read
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

    
    //[Authorize(Roles = "Admin")]
    [HttpDelete]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotifications()
    {
        await _repository.DeleteAllNotifications();
        return Ok();
    }

    //[Authorize(Roles = "Admin, Trainer, Client")]
    [HttpDelete("/user/{userId}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotificationsByUserTypeAndUserId(string userId)
    {
        return Ok( await _repository.DeleteNotificationsByUserId(userId));
    }

    //[Authorize(Roles = "Admin, Trainer, Client")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotification(string id)
    {
        return Ok( await _repository.DeleteNotification(id));
    }
}