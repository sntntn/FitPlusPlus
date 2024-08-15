using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TrainerService.API.Entities;
using TrainerService.API.GrpcServices;
using TrainerService.API.Repositories;

namespace TrainerService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerRepository _repository;
        private readonly ReviewGrpcService _reviewGrpcService;
        private readonly IMapper _mapper;


        public TrainerController(ITrainerRepository repository, ReviewGrpcService reviewGrpcService, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _reviewGrpcService = reviewGrpcService ?? throw new ArgumentNullException(nameof(reviewGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Trainer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainers()
        {
            var trainers = await _repository.GetTrainers();

            foreach (var trainer in trainers)
            {
                var reviews = await _reviewGrpcService.GetReviews(trainer.Id);
                trainer.Reviews = _mapper.Map<List<ReviewType>>(reviews.Reviews);
            }
            return Ok(trainers);
        }

        [HttpGet("{id}", Name = "GetTrainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Trainer>> GetTrainerById(string id)
        {
            var trainer = await _repository.GetTrainer(id);
            if (trainer == null)
            {
                return NotFound(null);
            }
            else
            {
                var reviews = await _reviewGrpcService.GetReviews(trainer.Id);
                trainer.Reviews = _mapper.Map<List<ReviewType>>(reviews.Reviews);
                return Ok(trainer);
            }
        }

        [Route("[action]/{minRating}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Trainer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainersByRating(double minRating)
        {
            var trainers = await _repository.GetTrainers();

            foreach (var trainer in trainers)
            {
                var reviews = await _reviewGrpcService.GetReviews(trainer.Id);
                trainer.Reviews = _mapper.Map<List<ReviewType>>(reviews.Reviews);
            }
            var filteredTrainers = trainers.Where(t => t.AverageRating >= minRating).ToList();
            return Ok(filteredTrainers);
        }

        [Route("[action]/{trainingType}")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Trainer>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Trainer>>> GetTrainersByTrainingType(string trainingType)
        {
            var trainers = await _repository.GetTrainersByTrainingType(trainingType);
            foreach (var trainer in trainers)
            {
                var reviews = await _reviewGrpcService.GetReviews(trainer.Id);
                trainer.Reviews = _mapper.Map<List<ReviewType>>(reviews.Reviews);

            }
            return Ok(trainers);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status201Created)]
        public async Task<ActionResult<Trainer>> CreateTrainer([FromBody] Trainer trainer)
        {

            foreach (var trainingType in trainer.TrainingTypes)
            {
                if (string.IsNullOrEmpty(trainingType.Id) || trainingType.Id == "")
                {
                    trainingType.Id = ObjectId.GenerateNewId().ToString();
                }
            }

            await _repository.CreateTrainer(trainer);


            return CreatedAtRoute("GetTrainer", new { id = trainer.Id }, trainer);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateTrainer([FromBody] Trainer trainer)
        {
            foreach (var trainingType in trainer.TrainingTypes)
            {
                if (string.IsNullOrEmpty(trainingType.Id) || trainingType.Id == "")
                {
                    trainingType.Id = ObjectId.GenerateNewId().ToString();
                }
            }

            return Ok(await _repository.UpdateTrainer(trainer));
        }

        [HttpDelete("{id}", Name = "DeleteTrainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteTrainer(string id)
        {
            return Ok(await _repository.DeleteTrainer(id));
        }
    }
}