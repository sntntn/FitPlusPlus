using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationService.API.Entities;
using ReservationService.API.Publishers;
using ReservationService.API.Repository;
// ReSharper disable All

namespace ReservationService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationRepository _repository;
    private readonly INotificationPublisher _notificationPublisher;

    public ReservationController(IReservationRepository repository, INotificationPublisher notificationPublisher)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _notificationPublisher = notificationPublisher ?? throw new ArgumentNullException(nameof(notificationPublisher));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("individual")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservations()
    {
        var reservations = await _repository.GetIndividualReservationsAsync();
        return Ok(reservations);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("group")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservations()
    {
        var reservations = await _repository.GetGroupReservationsAsync();
        return Ok(reservations);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("individual/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IndividualReservation>> GetIndividualReservation(string id)
    {
        var reservation = await _repository.GetIndividualReservationByIdAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("group/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupReservation>> GetGroupReservation(string id)
    {
        var reservation = await _repository.GetGroupReservationByIdAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }
    
    [Authorize(Roles = "Admin, Client")]
    [HttpGet("individual/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservationsByClientId(string clientId)
    {
        var reservations = await _repository.GetIndividualReservationsByClientIdAsync(clientId);
        return Ok(reservations);
    }
    
    [Authorize(Roles = "Admin, Client")]
    [HttpGet("group/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservationsByClientId(string clientId)
    {
        var reservations = await _repository.GetGroupReservationsByClientIdAsync(clientId);
        return Ok(reservations);
    }

    [Authorize(Roles = "Admin, Trainer")]
    [HttpGet("individual/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservationsByTrainerId(string trainerId)
    {
        var reservations = await _repository.GetIndividualReservationsByTrainerIdAsync(trainerId);
        return Ok(reservations);
    }
    
    [Authorize(Roles = "Admin, Trainer")]
    [HttpGet("group/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservationsByTrainerId(string trainerId)
    {
        var reservations = await _repository.GetGroupReservationsByTrainerIdAsync(trainerId);
        return Ok(reservations);
    }
    
    [Authorize(Roles = "Client")]
    [HttpPost("individual")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status201Created)]
    public async Task<ActionResult<IndividualReservation>> CreateIndividualReservation([FromBody] IndividualReservation reservation)
    {
        await _repository.CreateIndividualReservationAsync(reservation);

        var users = new Dictionary<string, string>();
        users.Add(reservation.ClientId, "Client");
        users.Add(reservation.TrainerId, "Trainer");
        await _notificationPublisher.PublishNotification("Training reservation created", reservation.ToString(), "Information", true, users);
        
        return CreatedAtRoute(nameof(GetIndividualReservation), new { id = reservation.Id }, reservation);
    }
    
    [Authorize(Roles = "Trainer")]
    [HttpPost("group")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status201Created)]
    public async Task<ActionResult<GroupReservation>> CreateGroupReservation([FromBody] GroupReservation reservation)
    {
        await _repository.CreateGroupReservationAsync(reservation);
        return CreatedAtRoute(nameof(GetGroupReservation), new { id = reservation.Id }, reservation);
    }
    
    [Authorize(Roles = "Client, Trainer")]
    [HttpPut("individual")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateIndividualReservation([FromBody] IndividualReservation reservation)
    {
        var updated = await _repository.UpdateIndividualReservationAsync(reservation);

        if (updated)
        {
            var users = new Dictionary<string, string>();
            users.Add(reservation.ClientId, "Client");
            users.Add(reservation.TrainerId, "Trainer");
            await _notificationPublisher.PublishNotification("Training reservation updated", reservation.ToString(),
                "Information", true, users);
        }

        return Ok(updated);
    }  
    
    [Authorize(Roles = "Trainer")]
    [HttpPut("group")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGroupReservation([FromBody] GroupReservation reservation)
    {
        var updated = await _repository.UpdateGroupReservationAsync(reservation);

        if (updated)
        {
            var users = new Dictionary<string, string>();
            foreach (var clientId in reservation.Clients)
            {
                users.Add(clientId, "Client");
            }
            users.Add(reservation.TrainerId, "Trainer");
            await _notificationPublisher.PublishNotification("Training reservation updated", reservation.ToString(),
                "Information", true, users);
        }

        return Ok(updated);
    }

    [Authorize(Roles = "Client, Trainer")]
    [HttpDelete("individual/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteIndividualReservation(string id)
    {
        var reservation = await _repository.GetIndividualReservationByIdAsync(id);
        var deleted = await _repository.DeleteIndividualReservationAsync(id);

        if (deleted)
        {
            var users = new Dictionary<string, string>
            {
                { reservation.ClientId, "Client" },
                { reservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation cancelled", reservation.ToString(),
                "Information", true, users);
        }

        return Ok(deleted);
    }

    [Authorize(Roles = "Trainer")]
    [HttpDelete("group/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGroupReservation(string id)
    {
        var reservation = await _repository.GetGroupReservationByIdAsync(id);
        var deleted = await _repository.UpdateGroupReservationAsync(reservation);

        if (deleted)
        {
            var users = new Dictionary<string, string>();
            foreach (var clientId in reservation.Clients)
            {
                users.Add(clientId, "Client");
            }
            users.Add(reservation.TrainerId, "Trainer");
            await _notificationPublisher.PublishNotification("Training reservation cancelled", reservation.ToString(),
                "Information", true, users);
        }

        return Ok(deleted);
    }

    [Authorize(Roles = "Client")]
    [HttpPost("group/book/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookGroupReservation(string id, [FromQuery] string clientId)
    {
        var reservation = await _repository.GetGroupReservationByIdAsync(id);
        var booked = await _repository.BookGroupReservationAsync(id, clientId);

        if (booked)
        {
            var users = new Dictionary<string, string>();
            users.Add(clientId, "Client");
            users.Add(reservation.TrainerId, "Trainer");
            await _notificationPublisher.PublishNotification("Training reservation booked", reservation.ToString(),
                "Information", true, users);
        }
        
        return Ok(booked);
    }

    [Authorize(Roles = "Client")]
    [HttpPost("group/cancel/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelGroupReservation(string id, [FromQuery] string clientId)
    { 
        var reservation = await _repository.GetGroupReservationByIdAsync(id);
        var cancelled = await _repository.CancelGroupReservationAsync(id, clientId);

        if (cancelled)
        {
            var users = new Dictionary<string, string>
            {
                { clientId, "Client" },
                { reservation.TrainerId, "Trainer" }
            };
            await _notificationPublisher.PublishNotification("Training reservation cancelled", reservation.ToString(),
                "Information", true, users);
        }
        
        return Ok(cancelled);
    }
}
