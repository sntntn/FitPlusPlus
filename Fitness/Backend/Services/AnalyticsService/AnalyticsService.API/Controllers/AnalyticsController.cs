using AnalyticsService.Common.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsRepository _repository;

    public AnalyticsController(IAnalyticsRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(double), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<double>> GetAverageRating([FromQuery] string trainerId)
    {
        var rating = await _repository.GetAverageRating(trainerId);
        return rating != 0.0 ? Ok(rating) : NotFound();
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetTrainerNumOfTrainings([FromQuery] string trainerId)
    {
        var numOfTrainings = await _repository.GetTrainerNumOfTrainings(trainerId);
        return Ok(numOfTrainings);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetClientNumOfTrainingForClient([FromQuery] string clientId)
    {
        var numOfTrainings = await _repository.GetClientNumOfTraining(clientId);
        return Ok(numOfTrainings);
    }
}