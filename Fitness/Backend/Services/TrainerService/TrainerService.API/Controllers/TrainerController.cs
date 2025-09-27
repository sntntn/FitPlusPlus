using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TrainerService.Common.Entities;
using TrainerService.Common.Repositories;
using TrainerService.API.GrpcServices;

namespace TrainerService.API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainerRepository _repository;
        private readonly ReviewGrpcService _reviewGrpcService;
        private readonly IMapper _mapper;
        private readonly IPublishEndpoint _publishEndpoint;

        public TrainerController(ITrainerRepository repository, ReviewGrpcService reviewGrpcService, IMapper mapper, IPublishEndpoint publishEndpoint)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _reviewGrpcService = reviewGrpcService ?? throw new ArgumentNullException(nameof(reviewGrpcService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }




        // [Authorize(Roles = "Admin, Client")]
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

        // [Authorize(Roles = "Admin, Client, Trainer")]
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

        // [Authorize(Roles = "Admin, Client, Trainer")]
        [HttpGet("[action]/{email}", Name = "GetTrainerByEmail")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Trainer>> GetTrainerByEmail(string email)
        {
            var trainer = await _repository.GetTrainerByEmail(email);
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

        // [Authorize(Roles = "Admin, Client")]
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

        // [Authorize(Roles = "Admin, Client")]
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

        // [Authorize(Roles = "Admin, Client, Trainer")]
        [Route("[action]/{trainerId}/{trainingType}")]
        [HttpGet]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        public async Task<ActionResult<decimal>> GetPrice(string trainerId, string trainingType)
        {
            var price = await _repository.GetPrice(trainerId,trainingType);

            return Ok(price);
        }

        // [Authorize(Roles = "Admin")]
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

        // [Authorize(Roles = "Admin, Trainer")]
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

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}", Name = "DeleteTrainer")]
        [ProducesResponseType(typeof(Trainer), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteTrainer(string id)
        {
            return Ok(await _repository.DeleteTrainer(id));
        }
    }
}