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

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Exercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises(string trainerId)
        {
            var exercises = await _repository.GetExercises(trainerId);
            return Ok(exercises);
        }

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Exercise>), StatusCodes.Status200OK)]
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

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Exercise>), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateExercise(Exercise exercise)
        {
            await _repository.CreateExercise(exercise);
            return CreatedAtRoute("GetExercise", new { id = exercise.Id} , exercise);
        }

        [Route("[action]")]
        [HttpPut]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateExercise(Exercise exercise)
        {
            var result = await _repository.UpdateExercise(exercise);
            return Ok(result);
        }
        
        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteExercise(string id)
        {
            var result = await _repository.DeleteExercise(id);
            return Ok(result);
        }

        
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsForClient(string clientId)
        {
            var trainings = _repository.GetTrainingsForClient(clientId);
            return Ok(trainings);
        }
        
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsForTrainer(string trainerId)
        {
            var trainings = _repository.GetTrainingsForTrainer(trainerId);
            return Ok(trainings);
        }
        
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
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
        
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTraining(Training training)
        {
            await _repository.CreateTraining(training);
            return CreatedAtRoute("GetTraining", new { id = training.TrainingId} , training);
        }

        [Route("[action]")]
        [HttpPut]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTraining(Training training)
        {
            var result = await _repository.UpdateTraining(training);
            return Ok(result);
        }
        
        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTraining(string id)
        {
            var result = await _repository.DeleteTraining(id);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TrainingExercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrainingExercise>>> GetTrainingExercises(string trainingId)
        {
            var trainingExercises = _repository.GetTrainingExercises(trainingId);
            return Ok(trainingExercises);
        }
        
        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TrainingExercise>), StatusCodes.Status200OK)]
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
        
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<TrainingExercise>), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTrainingExercise(TrainingExercise trainingExercise)
        {
            await _repository.CreateTrainingExercise(trainingExercise);
            return CreatedAtRoute("GetTrainingExercise", new { id = trainingExercise.Id} , trainingExercise);
        }

        [Route("[action]")]
        [HttpPut]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTrainingExercise(TrainingExercise trainingExercises)
        {
            var result = await _repository.UpdateTrainingExercise(trainingExercises);
            return Ok(result);
        }
        
        [Route("[action]")]
        [HttpDelete]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTrainingExercise(string id)
        {
            var result = await _repository.DeleteTrainingExercise(id);
            return Ok(result);
        }
    }
}