using System.Collections;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.API.Entities;
using NotificationService.API.Repositories;

namespace NotificationService.API.Controller;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController : ControllerBase
{
    private readonly IRepository _repository;
    private readonly IMapper _mapper;

    public NotificationController(IRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotifications()
    {
        var notifications = await _repository.GetNotifications();
        return Ok(notifications);
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("{userType}/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<Notification>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsByUserTypeAndUserId(string userType, string userId)
    {
        var notifications = await _repository.GetNotificationsByUserTypeAndUserId(userType, userId);
        return Ok(notifications);
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<ActionResult<Notification>> GetNotificationById(Guid id)
    {
        var notification = await _repository.GetNotificationById(id);
        return Ok(notification);
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpPut]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateNotification([FromBody] Notification notification)
    {
        return Ok(await _repository.UpdateNotification(notification));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotifications()
    {
        await _repository.DeleteAllNotifications();
        return Ok();
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpDelete("{userType}/{userId}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotificationsByUserTypeAndUserId(string userType, string userId)
    {
        return Ok( await _repository.DeleteNotificationsByUserTypeAndUserId(userType, userId));
    }

    [Authorize(Roles = "Admin, Trainer, Client")]
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(Notification), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNotification(Guid id)
    {
        return Ok( await _repository.DeleteNotification(id));
    }
}