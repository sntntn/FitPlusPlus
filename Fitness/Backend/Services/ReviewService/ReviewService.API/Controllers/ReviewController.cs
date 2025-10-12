using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReviewService.API.Publishers;
using ReviewService.Common.DTOs;
using ReviewService.Common.Repositories;

namespace ReviewService.API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _repository;
        private readonly IReviewPublisher _reviewPublisher;

        public ReviewController(IReviewRepository repository, IReviewPublisher reviewPublisher)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _reviewPublisher = reviewPublisher ?? throw new ArgumentNullException(nameof(reviewPublisher));
        }

        // [Authorize(Roles = "Admin, Trainer")]
        [HttpGet("trainer/{trainerId}", Name = "GetReviewsByTrainerId")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByTrainerId(string trainerId)
        {
            var reviews = await _repository.GetReviewsByTrainerId(trainerId);
            if (!reviews.Any())
            {
                return NotFound();
            }
            return Ok(reviews);
        }
        
        // [Authorize(Roles = "Admin, Client")]
        [HttpGet("client/{clientId}", Name = "GetReviewsByClientId")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetReviewsByClientId(string clientId)
        {
            var reviews = await _repository.GetReviewsByClientId(clientId);
            if (!reviews.Any())
            {
                return NotFound();
            }
            return Ok(reviews);
        }
        
        // [Authorize(Roles = "Trainer")]
        [HttpPost("trainer/{trainerId}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<ReviewDTO>> TrainerReview(string trainerId, [FromBody] SubmitReviewDTO reviewDTO)
        {
            var review = await _repository.GetReviewByReservationId(reviewDTO.ReservationId);
            if (review == null)
            {
                await _repository.CreateReview(reviewDTO.ReservationId, reviewDTO.ClientId, reviewDTO.TrainerId); 
            }
            var updated = await _repository.SubmitTrainerReview(reviewDTO);
            if (updated)
            { 
                await _reviewPublisher.PublishReview(reviewDTO, false);
            }
            return updated ? Ok(review) : BadRequest();
        }
        
        // [Authorize(Roles = "Client")]
        [HttpPost("client/{clientId}")]
        [ProducesResponseType(typeof(ReviewDTO), StatusCodes.Status201Created)]
        public async Task<ActionResult<ReviewDTO>> ClientReview(string clientId, [FromBody] SubmitReviewDTO reviewDTO)
        {
            var review = await _repository.GetReviewByReservationId(reviewDTO.ReservationId);
            if (review == null)
            {
                await _repository.CreateReview(reviewDTO.ReservationId, reviewDTO.ClientId, reviewDTO.TrainerId); 
            }
            var updated = await _repository.SubmitClientReview(reviewDTO);
            if (updated)
            { 
                await _reviewPublisher.PublishReview(reviewDTO, true);
            }
            return updated ? Ok(review) : BadRequest();
        }
    }
}