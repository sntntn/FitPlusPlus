using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.API.Controllers;

// [Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsRepository _repository;

    public AnalyticsController(IAnalyticsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }
    
    // Individual Trainings
    // [Authorize(Roles = "Admin, Trainer")]
    [HttpGet("individual/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualTraining>>> GetIndividualTrainingsByTrainerId(string trainerId)
    {
        var reservations = await _repository.GetIndividualTrainingsByTrainerId(trainerId);
        return Ok(reservations);
    }

    // [Authorize(Roles = "Admin, Client")]
    [HttpGet("individual/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualTraining>>> GetIndividualTrainingsByClientId(string clientId)
    {
        var reservations = await _repository.GetIndividualTrainingsByClientId(clientId);
        return Ok(reservations);
    }
    
    // Group Trainings
    // [Authorize(Roles = "Admin, Trainer")]
    [HttpGet("group/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupTraining>>> GetGroupTrainingsByTrainerId(string trainerId)
    {
        var reservations = await _repository.GetGroupTrainingsByTrainerId(trainerId);
        return Ok(reservations);
    }

    // [Authorize(Roles = "Admin, Client")]
    [HttpGet("group/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupTraining>>> GetGroupTrainingsByClientId(string clientId)
    {
        var reservations = await _repository.GetGroupTrainingsByClientId(clientId);
        return Ok(reservations);
    }
}