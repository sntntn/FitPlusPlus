using Microsoft.AspNetCore.Mvc;
using videoTrainingService.API.Entities;
using videoTrainingService.API.Repositories;

namespace videoTrainingService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class TrainingContoroller : ControllerBase
    {
        private readonly ITrainingRepository _repository;

        public TrainingContoroller(ITrainingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Exercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises(string trainerId)
        {
            var exercises = await _repository.GetExercises(trainerId);
            return Ok(exercises);
        }
    }
}