using AnalyticsService.API.GrpcServices;
using AnalyticsService.Common.Entities;
using AnalyticsService.Common.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsRepository _repository;
    private readonly ReviewGrpcService _reviewGrpcService;
    private readonly IMapper _mapper;

    public AnalyticsController(IAnalyticsRepository repository, ReviewGrpcService reviewGrpcService, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _reviewGrpcService = reviewGrpcService ?? throw new ArgumentNullException(nameof(reviewGrpcService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreateTraining([FromQuery] Training training)
    {
        await _repository.CreateTraining(training);
        return Created();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult> DeleteTraining([FromQuery] string id)
    {
        return Ok(await _repository.DeleteTraining(id));
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(double), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(double), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<double>> GetTrainerAverageTrainingRating([FromQuery] string trainerId)
    {
        var rating = await _repository.GetTrainerAverageTrainingRating(trainerId);
        return rating != 0.0 ? Ok(rating) : NotFound();
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetTrainerNumOfTrainings([FromQuery] string trainerId)
    {
        var numOfTrainings = await _repository.GetTrainerNumOfTrainings(trainerId);
        return Ok(numOfTrainings);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetClientNumOfTrainingForClient([FromQuery] string clientId)
    {
        var numOfTrainings = await _repository.GetClientNumOfTrainings(clientId);
        return Ok(numOfTrainings);
    }
    
    [HttpGet("[action]")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetClientNumOfHeldTrainings([FromQuery] string clientId)
    {
        var numOfHeldTrainings = await _repository.GetClientNumOfHeldTrainings(clientId);
        return Ok(numOfHeldTrainings);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    public async Task<ActionResult<int>> GetClientNumOfCancelledTrainings([FromQuery] string clientId)
    {
        var numOfCancelledTrainings = await _repository.GetClientNumOfCancelledTrainings(clientId);
        return Ok(numOfCancelledTrainings);
    }

    [HttpGet("[action]")]
    [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<string>>> GetTrainerClientIds([FromQuery] string trainerId)
    {
        var clientIds = await _repository.GetTrainerClientIds(trainerId);
        return Ok(clientIds);
    }

    [HttpGet("TrainerReviews")]
    [ProducesResponseType(typeof(IEnumerable<ReviewType>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ReviewType>>> GetTrainerReviews([FromQuery] string trainerId)
    {
        var response = await _reviewGrpcService.GetReviews(trainerId);
        var reviews = _mapper.Map<IEnumerable<ReviewType>>(response.Reviews);
        return Ok(reviews);
    }
}