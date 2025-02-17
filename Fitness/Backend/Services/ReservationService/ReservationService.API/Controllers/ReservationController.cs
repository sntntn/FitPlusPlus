using AutoMapper;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReservationService.API.Entities;
using ReservationService.API.Repository;

namespace ReservationService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class ReservationController : ControllerBase
{
    private readonly IReservationRepository _repository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public ReservationController(IReservationRepository repository, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
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
        return Ok(await _repository.UpdateIndividualReservationAsync(reservation));
    }  
    
    [Authorize(Roles = "Trainer")]
    [HttpPut("group")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateGroupReservation([FromBody] GroupReservation reservation)
    {
        return Ok(await _repository.UpdateGroupReservationAsync(reservation));
    }

    [Authorize(Roles = "Client, Trainer")]
    [HttpDelete("individual/{id}")]
    [ProducesResponseType(typeof(IndividualReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteIndividualReservation(string id)
    {
        return Ok(await _repository.DeleteIndividualReservationAsync(id));
    }

    [Authorize(Roles = "Trainer")]
    [HttpDelete("group/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteGroupReservation(string id)
    {
        return Ok(await _repository.DeleteGroupReservationAsync(id));
    }

    [Authorize(Roles = "Client")]
    [HttpPost("group/book/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> BookGroupReservation(string id, [FromQuery] string clientId)
    {
        return Ok(await _repository.BookGroupReservationAsync(id, clientId));
    }

    [Authorize(Roles = "Client")]
    [HttpPost("group/cancel/{id}")]
    [ProducesResponseType(typeof(GroupReservation), StatusCodes.Status200OK)]
    public async Task<IActionResult> CancelGroupReservation(string id, [FromQuery] string clientId)
    { 
        return Ok(await _repository.CancelGroupReservationAsync(id, clientId));
    }
}
