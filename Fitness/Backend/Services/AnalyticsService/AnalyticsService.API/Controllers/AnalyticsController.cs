using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.API.Controllers;

/// <summary>
/// Provides REST API endpoints for getting all reserved trainings along with the corresponding reviews.
/// </summary>
/// <remarks>
/// This controller exposes operations for fetching individual and group trainings.
///
/// All routes are secured and require authorization with roles <c>Admin</c>, <c>Trainer</c>, or <c>Client</c>.
/// </remarks>
[Authorize]
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
    /// <summary>
    /// API for fetching all individual trainings associated with a given trainer.
    /// </summary>
    /// <param name="trainerId">Unique identifier of the desired trainer</param>
    /// <returns><c>IEnumerable</c> of individual trainings for the given trainer.</returns>
    [Authorize(Roles = "Admin, Trainer")]
    [HttpGet("individual/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualTraining>>> GetIndividualTrainingsByTrainerId(string trainerId)
    {
        var reservations = await _repository.GetIndividualTrainingsByTrainerId(trainerId);
        return Ok(reservations);
    }

    /// <summary>
    /// API for fetching all individual trainings associated with a given client.
    /// </summary>
    /// <param name="clientId">Unique identifier of the desired client</param>
    /// <returns><c>IEnumerable</c> of individual trainings for the given client.</returns>
    [Authorize(Roles = "Admin, Client")]
    [HttpGet("individual/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<IndividualTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<IndividualTraining>>> GetIndividualTrainingsByClientId(string clientId)
    {
        var reservations = await _repository.GetIndividualTrainingsByClientId(clientId);
        return Ok(reservations);
    }
    
    // Group Trainings
    /// <summary>
    /// API for fetching all group trainings associated with a given trainer.
    /// </summary>
    /// <param name="trainerId">Unique identifier of the desired trainer</param>
    /// <returns><c>IEnumerable</c> of group trainings for the given trainer.</returns>
    [Authorize(Roles = "Admin, Trainer")]
    [HttpGet("group/trainer/{trainerId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupTraining>>> GetGroupTrainingsByTrainerId(string trainerId)
    {
        var reservations = await _repository.GetGroupTrainingsByTrainerId(trainerId);
        return Ok(reservations);
    }

    /// <summary>
    /// API for fetching all group trainings associated with a given client.
    /// </summary>
    /// <param name="clientId">Unique identifier of the desired client</param>
    /// <returns><c>IEnumerable</c> of group trainings for the given client.</returns>
    [Authorize(Roles = "Admin, Client")]
    [HttpGet("group/client/{clientId}")]
    [ProducesResponseType(typeof(IEnumerable<GroupTraining>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GroupTraining>>> GetGroupTrainingsByClientId(string clientId)
    {
        var reservations = await _repository.GetGroupTrainingsByClientId(clientId);
        return Ok(reservations);
    }
}