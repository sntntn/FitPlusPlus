using Microsoft.AspNetCore.Mvc;
using videoTrainingService.API.Entities;
using videoTrainingService.API.Repositories;

namespace videoTrainingService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepository _repository;

        public TrainingController(ITrainingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        
        [HttpGet("exercises/{trainerId}")]
        [ProducesResponseType(typeof(IEnumerable<Exercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises(string trainerId)
        {
            var exercises = await _repository.GetExercises(trainerId);
            return Ok(exercises);
        }
        
        [HttpGet("exercise/{id}", Name = "GetExercise")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Exercise>> GetExercise(string id)
        {
            var exercise = await _repository.GetExercise(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return Ok(exercise);
        }

        [HttpPost("exercise")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateExercise([FromBody] Exercise exercise)
        {
            await _repository.CreateExercise(exercise);
            return CreatedAtRoute("GetExercise", new { id = exercise.Id} , exercise);
        }

        [HttpPut("exercise")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateExercise([FromBody] Exercise exercise)
        {
            var result = await _repository.UpdateExercise(exercise);
            return Ok(result);
        }
        
        [HttpDelete("exercise/{id}")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteExercise(string id)
        {
            var result = await _repository.DeleteExercise(id);
            return Ok(result);
        }
        
        [HttpGet("training/trainingClient/{clientId}", Name = "GetTrainingsForClient")]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsForClient(string clientId)
        {
            var trainings = await _repository.GetTrainingsForClient(clientId);
            return Ok(trainings);
        }
        
        [HttpGet("training/trainingTrainer/{trainerId}", Name = "GetTrainingsForTrainer")]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsForTrainer(string trainerId)
        {
            var trainings = await _repository.GetTrainingsForTrainer(trainerId);
            return Ok(trainings);
        }
        
        [HttpGet("training/{id}", Name = "GetTraining")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Training>> GetTraining(string id)
        {
            var training = await _repository.GetTraining(id);
            if (training == null)
            {
                return NotFound();
            }
            return Ok(training);
        }
        
        [HttpPost("training")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTraining([FromBody] Training training)
        {
            await _repository.CreateTraining(training);
            return CreatedAtRoute("GetTraining", new { id = training.TrainingId} , training);
        }

        [HttpPut("training")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTraining([FromBody] Training training)
        {
            var result = await _repository.UpdateTraining(training);
            return Ok(result);
        }
        
        [HttpDelete("training/{id}")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTraining(string id)
        {
            var result = await _repository.DeleteTraining(id);
            return Ok(result);
        }

        [HttpGet("trainingExercises/{trainingId}", Name = "GetTrainingExercises")]
        [ProducesResponseType(typeof(IEnumerable<TrainingExercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrainingExercise>>> GetTrainingExercises(string trainingId)
        {
            var trainingExercises = await _repository.GetTrainingExercises(trainingId);
            return Ok(trainingExercises);
        }
        
        [HttpGet("trainingExercise/{id}", Name = "GetTrainingExercise")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TrainingExercise>> GetTrainingExercise(string id)
        {
            var trainingExercise = await _repository.GetTrainingExercise(id);
            if (trainingExercise == null)
            {
                return NotFound();
            }
            return Ok(trainingExercise);
        }
        
        [HttpPost("trainingExercise")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTrainingExercise([FromBody] TrainingExercise trainingExercise)
        {
            await _repository.CreateTrainingExercise(trainingExercise);
            return CreatedAtRoute("GetTrainingExercise", new { id = trainingExercise.Id} , trainingExercise);
        }
        
        [HttpPut("trainingExercise")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTrainingExercise([FromBody] TrainingExercise trainingExercises)
        {
            var result = await _repository.UpdateTrainingExercise(trainingExercises);
            return Ok(result);
        }
        
        [HttpDelete("trainingExercise/{id}")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTrainingExercise(string id)
        {
            var result = await _repository.DeleteTrainingExercise(id);
            return Ok(result);
        }
    }
}