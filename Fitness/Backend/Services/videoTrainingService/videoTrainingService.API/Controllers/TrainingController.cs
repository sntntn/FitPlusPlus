using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using videoTrainingService.API.Entities;
using videoTrainingService.API.Repositories;

namespace videoTrainingService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    
    public class TrainingController : ControllerBase
    {
        private readonly ITrainingRepository _repository;

        public TrainingController(ITrainingRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// API for retrieval of added exercises from specific trainer.
        /// </summary> 
        /// <param name="trainerId">Unique identifier of the desired trainer.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("exercises/{trainerId}")]
        [ProducesResponseType(typeof(IEnumerable<Exercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises(string trainerId)
        {
            var exercises = await _repository.GetExercises(trainerId);
            return Ok(exercises);
        }
        
        /// <summary>
        /// API for retrieval of one specific exercise.
        /// </summary> 
        /// <param name="id">Unique identifier of the desired exercise.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
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

        /// <summary>
        /// API for addition of new exercises by trainer.
        /// </summary> 
        /// <param name="exercise">Object that represents exercise and contains values for exercise name and video path.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost("exercise")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateExercise([FromBody] Exercise exercise)
        {
            await _repository.CreateExercise(exercise);
            return CreatedAtRoute("GetExercise", new { id = exercise.Id} , exercise);
        }

        /// <summary>
        /// API for exercise update.
        /// </summary> 
        /// <param name="exercise">Object that represents exercise with updated values.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut("exercise")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateExercise([FromBody] Exercise exercise)
        {
            var result = await _repository.UpdateExercise(exercise);
            return Ok(result);
        }
        
        /// <summary>
        /// API for exercise removal.
        /// </summary> 
        /// <param name="id">Unique identifier of the desired exercise.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete("exercise/{id}")]
        [ProducesResponseType(typeof(Exercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteExercise(string id)
        {
            var result = await _repository.DeleteExercise(id);
            return Ok(result);
        }
        
        /// <summary>
        /// API for retrieval of trainings for clients. Retrives all trainings in database.
        /// </summary> 
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("training/trainingClient", Name = "GetTrainingsForClient")]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsForClient()
        {
            var trainings = await _repository.GetTrainingsForClient();
            return Ok(trainings);
        }
        
        /// <summary>
        /// API for retrieval of trainings for trainer.
        /// </summary>
        /// <param name="trainerId">Unique identifier of the desired trainer.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("training/trainingTrainer/{trainerId}", Name = "GetTrainingsForTrainer")]
        [ProducesResponseType(typeof(IEnumerable<Training>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Training>>> GetTrainingsForTrainer(string trainerId)
        {
            var trainings = await _repository.GetTrainingsForTrainer(trainerId);
            return Ok(trainings);
        }
        
        /// <summary>
        /// API for retrieval of specific training.
        /// </summary>
        /// <param name="id">Unique identifier of the desired training.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
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
        
        /// <summary>
        /// API for addition of new training made by trainer.
        /// </summary> 
        /// <param name="training">Object that represents training with values for trainer id, 
        /// training type and description.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost("training")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTraining([FromBody] Training training)
        {
            await _repository.CreateTraining(training);
            return CreatedAtRoute("GetTraining", new { id = training.TrainingId} , training);
        }

        /// <summary>
        /// API for training update.
        /// </summary> 
        /// <param name="training">Object with updated values.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut("training")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTraining([FromBody] Training training)
        {
            var result = await _repository.UpdateTraining(training);
            return Ok(result);
        }
        
        /// <summary>
        /// API for training removal.
        /// </summary> 
        /// <param name="id">Unique identifier of the desired training.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete("training/{id}")]
        [ProducesResponseType(typeof(Training), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTraining(string id)
        {
            var result = await _repository.DeleteTraining(id);
            return Ok(result);
        }

        /// <summary>
        /// API for retrieval of all exercises from specific training.
        /// </summary> 
        /// <param name="trainingId">Unique identifier of the desired training.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("trainingExercises/{trainingId}", Name = "GetTrainingExercises")]
        [ProducesResponseType(typeof(IEnumerable<TrainingExercise>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TrainingExercise>>> GetTrainingExercises(string trainingId)
        {
            var trainingExercises = await _repository.GetTrainingExercises(trainingId);
            return Ok(trainingExercises);
        }
        
        /// <summary>
        /// API for retrieval of one exercise from specific training.
        /// </summary> 
        /// <param name="id">Unique identifier of the desired exercise.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
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
        
        /// <summary>
        /// API for addition of new exercise in specific training.
        /// </summary> 
        /// <param name="trainingExercises">Object that represents exercise and contains values for training and existing exercise id and sets and reps 
        /// number of repetitions.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost("trainingExercise")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status201Created)]
        public async Task<ActionResult> CreateTrainingExercise([FromBody] TrainingExercise trainingExercise)
        {
            await _repository.CreateTrainingExercise(trainingExercise);
            return CreatedAtRoute("GetTrainingExercise", new { id = trainingExercise.Id} , trainingExercise);
        }
        
        /// <summary>
        /// API for training update.
        /// </summary> 
        /// <param name="trainingExercises">Object that represents exercise with updated values.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPut("trainingExercise")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTrainingExercise([FromBody] TrainingExercise trainingExercises)
        {
            var result = await _repository.UpdateTrainingExercise(trainingExercises);
            return Ok(result);
        }
        
        /// <summary>
        /// API for removal of exercises from training. Removes all exercises for desired training.
        /// </summary> 
        /// <param name="trainingId">Unique identifier of the desired training.</param>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpDelete("trainingExercise/{trainingId}")]
        [ProducesResponseType(typeof(TrainingExercise), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteTrainingExercises(string trainingId)
        {
            var result = await _repository.DeleteTrainingExercises(trainingId);
            return Ok(result);
        }
        
        /// <summary>
        /// API for training's queue update. Adds client's unique identifier in queue of clients who purchased this training.
        /// </summary> 
        /// <param name="clientId">Unique identifier of client who purchased training.</param>
        /// <param name="trainingId">Unique identifier of purchased training.</param>
        [Authorize(Roles = "Client")]
        [HttpPost("training/{trainingId}/addClient/{clientId}")]
        public async Task<IActionResult> AddClientToTraining(string trainingId, string clientId)
        {
            var success = await _repository.AddClientToTraining(trainingId, clientId);

            if (!success)
                return BadRequest("Client could not be added to training (maybe already exists or training not found).");

            return Ok($"Client {clientId} added to training {trainingId}.");
        }

        /// <summary>
        /// API for retrieval of purchased trainings for client.
        /// </summary> 
        /// <param name="clientId">Unique identifier of desired client.</param>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("training/byClient/{clientId}")]
        public async Task<ActionResult<List<string>>> GetTrainingsByClient(string clientId)
        {
            var trainings = await _repository.GetTrainingsByClient(clientId);

            if (trainings == null || trainings.Count == 0)
                return NotFound($"No trainings found for client {clientId}.");

            return Ok(trainings);
        }
    }
}