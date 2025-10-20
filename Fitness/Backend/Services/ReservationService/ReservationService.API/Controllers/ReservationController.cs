using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationService.API.Entities;
using ReservationService.API.Services;

// ReSharper disable All

namespace ReservationService.API.Controllers;

/// <summary>
/// Provides REST API endpoints for fetching, creating, and managing reservation data.
/// </summary>
/// <remarks>
/// This controller exposes operations for handling both individual and group reservations.
///
/// All routes are secured and require authorization with roles <c>Admin</c>, <c>Trainer</c>, or <c>Client</c>.
/// </remarks>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService));
    }

    /// <summary>
    /// API for fetching all individual reservations.
    /// </summary>
    /// <returns><c>IEnumerable</c> of all individual reservations.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("individual")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservations()
    {
        var reservations = await _reservationService.GetIndividualReservationsAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// API for fetching all group reservations.
    /// </summary>
    /// <returns><c>IEnumerable</c> of all group reservations.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("group")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservations()
    {
        var reservations = await _reservationService.GetGroupReservationsAsync();
        return Ok(reservations);
    }

    /// <summary>
    /// API for fetching a specific individual reservation by its identifier.
    /// </summary>
    /// <param name="id">Unique identifier of the individual reservation.</param>
    /// <returns>Individual reservation with the given identifier.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("individual/{id}", Name = "GetIndividualReservation")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IndividualReservation>> GetIndividualReservation(string id)
    {
        var reservation = await _reservationService.GetIndividualReservationAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    /// <summary>
    /// API for fetching a specific group reservation by its identifier.
    /// </summary>
    /// <param name="id">Unique identifier of the group reservation.</param>
    /// <returns>Group reservation with the given identifier.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("group/{id}", Name = "GetGroupReservation")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupReservation>> GetGroupReservation(string id)
    {
        var reservation = await _reservationService.GetGroupReservationAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    /// <summary>
    /// API for fetching all individual reservations for a specific client.
    /// </summary>
    /// <param name="clientId">Unique identifier of the client.</param>
    /// <returns><c>IEnumerable</c> of individual reservations for the given client.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("individual/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservationsByClientId(string clientId)
    {
        var reservations = await _reservationService.GetIndividualReservationsByClientIdAsync(clientId);
        return Ok(reservations);
    }
    
    /// <summary>
    /// API for fetching all group reservations for a specific client.
    /// </summary>
    /// <param name="clientId">Unique identifier of the client.</param>
    /// <returns><c>IEnumerable</c> of group reservations for the given client.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("group/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservationsByClientId(string clientId)
    {
        var reservations = await _reservationService.GetGroupReservationsByClientIdAsync(clientId);
        return Ok(reservations);
    }

    /// <summary>
    /// API for fetching all individual reservations for a specific trainer.
    /// </summary>
    /// <param name="trainerId">Unique identifier of the trainer.</param>
    /// <returns><c>IEnumerable</c> of individual reservations for the given trainer.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("individual/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservationsByTrainerId(string trainerId)
    {
        var reservations = await _reservationService.GetIndividualReservationsByTrainerIdAsync(trainerId);
        return Ok(reservations);
    }

    /// <summary>
    /// API for fetching all group reservations for a specific trainer.
    /// </summary>
    /// <param name="trainerId">Unique identifier of the trainer.</param>
    /// <returns><c>IEnumerable</c> of group reservations for the given trainer.</returns>
    [Authorize(Roles = "Admin, Client, Trainer")]
    [HttpGet("group/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservationsByTrainerId(string trainerId)
    {
        var reservations = await _reservationService.GetGroupReservationsByTrainerIdAsync(trainerId);
        return Ok(reservations);
    }

    /// <summary>
    /// API for creating a new individual reservation.
    /// </summary>
    /// <param name="reservation">Individual reservation data to be created.</param>
    /// <returns>201 Created if successful.</returns>
    [Authorize(Roles = "Client")]
    [HttpPost("individual")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status201Created)]
    public async Task<ActionResult<IndividualReservation>> CreateIndividualReservation([FromBody] IndividualReservation reservation)
    {
        var created = await _reservationService.CreateIndividualReservationAsync(reservation);
        if (created)
        {
            return CreatedAtRoute(nameof(GetIndividualReservation), new { id = reservation.Id }, reservation);
        }
        else
        {
            return BadRequest();
        }
    }
    
    /// <summary>
    /// API for creating a new group reservation.
    /// </summary>
    /// <param name="reservation">Group reservation data to be created.</param>
    /// <returns>201 Created if successful.</returns>
    [Authorize(Roles = "Trainer")]
    [HttpPost("group")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status201Created)]
    public async Task<ActionResult<GroupReservation>> CreateGroupReservation([FromBody] GroupReservation reservation)
    {
        var created = await _reservationService.CreateGroupReservationAsync(reservation);
        if (created)
        {
            return CreatedAtRoute(nameof(GetGroupReservation), new { id = reservation.Id }, reservation);
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// API for deleting a group reservation by its identifier.
    /// </summary>
    /// <param name="id">Unique identifier of the group reservation.</param>
    /// <returns>204 NoContent if deleted successfully.</returns>
    [Authorize(Roles = "Trainer")]
    [HttpDelete("group/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGroupReservation(string id)
    {
        var deleted = await _reservationService.DeleteGroupReservationAsync(id);
        if (deleted)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// API for cancelling an individual reservation by the client.
    /// </summary>
    /// <param name="id">Unique identifier of the reservation to cancel.</param>
    /// <returns>204 NoContent if cancelled successfully.</returns>
    [Authorize(Roles = "Client")]
    [HttpPut("individual/client/cancel/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelClientIndividualReservation(string id)
    {
        var cancelled = await _reservationService.ClientCancelIndividualReservationAsync(id);
        return cancelled ? Ok(cancelled) : BadRequest();
    }

    /// <summary>
    /// API for cancelling an individual reservation by the trainer.
    /// </summary>
    /// <param name="id">Unique identifier of the reservation to cancel.</param>
    /// <returns>204 NoContent if cancelled successfully.</returns>
    [Authorize(Roles = "Trainer")]
    [HttpPut("individual/trainer/cancel/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelTrainerIndividualReservation(string id)
    {
        var cancelled = await _reservationService.TrainerCancelIndividualReservationAsync(id);
        return cancelled ? Ok(cancelled) : BadRequest();
    }

    /// <summary>
    /// API for booking a group reservation by a client.
    /// </summary>
    /// <param name="id">Unique identifier of the group reservation.</param>
    /// <param name="clientId">Unique identifier of the client booking the reservation.</param>
    /// <returns>200 OK if booked successfully.</returns>
    [Authorize(Roles = "Client")]
    [HttpPost("group/book/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookGroupReservation(string id, [FromQuery] string clientId)
    {
        var booked = await _reservationService.BookGroupReservationAsync(id, clientId);
        if (booked)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    /// <summary>
    /// API for cancelling a group reservation by a client.
    /// </summary>
    /// <param name="id">Unique identifier of the group reservation.</param>
    /// <param name="clientId">Unique identifier of the client cancelling the reservation.</param>
    /// <returns>204 NoContent if cancelled successfully.</returns>
    [Authorize(Roles = "Client")]
    [HttpPost("group/cancel/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelGroupReservation(string id, [FromQuery] string clientId)
    { 
        var cancelled = await _reservationService.CancelGroupReservationAsync(id, clientId);
        if (cancelled)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }
}
