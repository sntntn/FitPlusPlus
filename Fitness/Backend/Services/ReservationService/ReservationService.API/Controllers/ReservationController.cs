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
    public async Task<ActionResult<IEnumerable<IndividualReservation>>> GetIndividualReservations([FromQuery] string? clientId, [FromQuery] string? trainerId)
    {
        var reservations = await _repository.GetIndividualReservationsAsync(clientId, trainerId);
        return Ok(reservations);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("group")]
    [ProducesResponseType(typeof(IEnumerable<GroupReservation>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupReservation>>> GetGroupReservations([FromQuery] string? clientId, [FromQuery] string? trainerId)
    {
        var reservations = await _repository.GetGroupReservationsAsync(clientId, trainerId);
        return Ok(reservations);
    }

    [HttpGet("individual/{id}")]
    public async Task<ActionResult<IndividualReservation>> GetIndividualReservation(string id)
    {
        var reservation = await _repository.GetIndividualReservationByIdAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    [HttpGet("group/{id}")]
    public async Task<ActionResult<GroupReservation>> GetGroupReservation(string id)
    {
        var reservation = await _repository.GetGroupReservationByIdAsync(id);
        if (reservation == null) return NotFound();
        return Ok(reservation);
    }

    [HttpPost("individual")]
    public async Task<ActionResult> CreateIndividualReservation([FromBody] IndividualReservation reservation)
    {
        await _repository.CreateIndividualReservationAsync(reservation);
        return CreatedAtAction(nameof(GetIndividualReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPost("group")]
    public async Task<ActionResult> CreateGroupReservation([FromBody] GroupReservation reservation)
    {
        await _repository.CreateGroupReservationAsync(reservation);
        return CreatedAtAction(nameof(GetGroupReservation), new { id = reservation.Id }, reservation);
    }

    [HttpPut("individual/{id}")]
    public async Task<IActionResult> UpdateIndividualReservation(string id, [FromBody] IndividualReservation reservation)
    {
        await _repository.UpdateIndividualReservationAsync(id, reservation);
        return NoContent();
    }

    [HttpPut("group/{id}")]
    public async Task<IActionResult> UpdateGroupReservation(string id, [FromBody] GroupReservation reservation)
    {
        await _repository.UpdateGroupReservationAsync(id, reservation);
        return NoContent();
    }

    [HttpDelete("individual/{id}")]
    public async Task<IActionResult> DeleteIndividualReservation(string id)
    {
        await _repository.DeleteIndividualReservationAsync(id);
        return NoContent();
    }

    [HttpDelete("group/{id}")]
    public async Task<IActionResult> DeleteGroupReservation(string id)
    {
        await _repository.DeleteGroupReservationAsync(id);
        return NoContent();
    }

    [Authorize(Roles = "Client")]
    [HttpPost("group/book/{id}")]
    public async Task<IActionResult> BookGroupReservation(string id, [FromQuery] string clientId)
    {
        await _repository.BookGroupReservationAsync(id, clientId);
        return Ok();
    }

    [Authorize(Roles = "Client")]
    [HttpPost("group/cancel/{id}")]
    public async Task<IActionResult> CancelGroupReservation(string id, [FromQuery] string clientId)
    {
        await _repository.CancelGroupReservationAsync(id, clientId);
        return Ok();
    }
}