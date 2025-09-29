using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationService.API.Entities;
using ReservationService.API.Services;

// ReSharper disable All

namespace ReservationService.API.Controllers;

//[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService ?? throw new ArgumentNullException(nameof(reservationService)); 
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet("individual")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservations()
    {
        var reservations = await _reservationService.GetIndividualReservations();
        return Ok(reservations);
    }

    //[Authorize(Roles = "Admin")]
    [HttpGet("group")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservations()
    {
        var reservations = await _reservationService.GetGroupReservations();
        return Ok(reservations);
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpGet("individual/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IndividualReservation>> GetIndividualReservation(string id)
    {
        var reservation = await _reservationService.GetIndividualReservation(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }
    
    //[Authorize(Roles = "Admin")]
    [HttpGet("group/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GroupReservation>> GetGroupReservation(string id)
    {
        var reservation = await _reservationService.GetGroupReservation(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }
    
    //[Authorize(Roles = "Admin, Client")]
    [HttpGet("individual/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservationsByClientId(string clientId)
    {
        var reservations = await _reservationService.GetIndividualReservationsByClientId(clientId);
        return Ok(reservations);
    }
    
    //[Authorize(Roles = "Admin, Client")]
    [HttpGet("group/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservationsByClientId(string clientId)
    {
        var reservations = await _reservationService.GetGroupReservationsByClientId(clientId);
        return Ok(reservations);
    }

    //[Authorize(Roles = "Admin, Trainer")]
    [HttpGet("individual/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservationsByTrainerId(string trainerId)
    {
        var reservations = await _reservationService.GetIndividualReservationsByTrainerId(trainerId);
        return Ok(reservations);
    }
    
    //[Authorize(Roles = "Admin, Trainer")]
    [HttpGet("group/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservationsByTrainerId(string trainerId)
    {
        var reservations = await _reservationService.GetGroupReservationsByTrainerId(trainerId);
        return Ok(reservations);
    }
    
    //[Authorize(Roles = "Client")]
    [HttpPost("individual")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status201Created)]
    public async Task<ActionResult<IndividualReservation>> CreateIndividualReservation([FromBody] IndividualReservation reservation)
    {
        var created = await _reservationService.CreateIndividualReservation(reservation);
        if (created)
        {
            return CreatedAtRoute(nameof(GetIndividualReservation), new { id = reservation.Id }, reservation);
        }
        else
        {
            return BadRequest();
        }
    }
    
    //[Authorize(Roles = "Trainer")]
    [HttpPost("group")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status201Created)]
    public async Task<ActionResult<GroupReservation>> CreateGroupReservation([FromBody] GroupReservation reservation)
    {
        var created = await _reservationService.CreateGroupReservation(reservation);
        if (created)
        {
            return CreatedAtRoute(nameof(GetIndividualReservation), new { id = reservation.Id }, reservation);
        }
        else
        {
            return BadRequest();
        }
    }
    
    //[Authorize(Roles = "Client, Trainer")]
    [HttpPut("individual")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateIndividualReservation([FromBody] IndividualReservation reservation)
    {
        var updated = await _reservationService.UpdateIndividualReservation(reservation);
        if (updated)
        {
            return Ok(reservation);
        }
        else
        {
            return BadRequest();
        }
    }  
    
    //[Authorize(Roles = "Trainer")]
    [HttpPut("group")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGroupReservation([FromBody] GroupReservation reservation)
    {
        var updated = await _reservationService.UpdateGroupReservation(reservation);
        if (updated)
        {
            return Ok(reservation);
        }
        else
        {
            return  BadRequest();
        }
    }

    //[Authorize(Roles = "Client, Trainer")]
    [HttpDelete("individual/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteIndividualReservation(string id)
    {
        var deleted =  await _reservationService.DeleteIndividualReservation(id);
        if (deleted)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    //[Authorize(Roles = "Trainer")]
    [HttpDelete("group/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGroupReservation(string id)
    {
        var deleted =  await _reservationService.DeleteGroupReservation(id);
        if (deleted)
        {
            return NoContent();
        }
        else
        {
            return BadRequest();
        }
    }

    //[Authorize(Roles = "Client")]
    [HttpPost("group/book/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookGroupReservation(string id, [FromQuery] string clientId)
    {
        var booked = await _reservationService.BookGroupReservation(id, clientId);
        if (booked)
        {
            return Ok();
        }
        else
        {
            return BadRequest();
        }
    }

    //[Authorize(Roles = "Client")]
    [HttpPost("group/cancel/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CancelGroupReservation(string id, [FromQuery] string clientId)
    { 
        var cancelled = await _reservationService.CancelGroupReservation(id, clientId);
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
