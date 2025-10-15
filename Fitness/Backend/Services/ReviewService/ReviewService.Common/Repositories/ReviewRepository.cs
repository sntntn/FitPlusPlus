using AutoMapper;
using MongoDB.Driver;
using ReviewService.Common.Data;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;

namespace ReviewService.Common.Repositories
{
    internal class ReviewRepository : IReviewRepository
    {
        private readonly IReviewContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(IReviewContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ReviewDTO?> GetReviewByReservationId(string reservationId)
        {
            var review = await _context.Reviews.Find(r => r.ReservationId == reservationId).FirstOrDefaultAsync();
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<ReviewDTO?> GetReviewByReservationIdClientId(string reservationId, string clientId)
        {
            var review = await _context.Reviews
                .Find(r => r.ReservationId == reservationId && r.ClientId == clientId)
                .FirstOrDefaultAsync();
            return _mapper.Map<ReviewDTO>(review);
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByTrainerId(string trainerId)
        {
            var reviews = await _context.Reviews
                .Find(r => r.TrainerId == trainerId && r.ClientComment != null && r.ClientRating != null)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task<IEnumerable<ReviewDTO>> GetReviewsByClientId(string clientId)
        {
            var reviews = await _context.Reviews
                .Find(r => r.ClientId == clientId && r.TrainerComment != null && r.TrainerRating != null)
                .ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task CreateReview(string reservationId, string clientId, string trainerId)
        {
            var review = new ReviewDTO { ReservationId = reservationId, ClientId = clientId, TrainerId = trainerId};
            await _context.Reviews.InsertOneAsync(_mapper.Map<Review>(review));
        }

        public async Task<bool> SubmitClientReview(SubmitReviewDTO clientReview)
        {
            var existingTrainerReview = await _context.Reviews
                .Find(r => r.ReservationId == clientReview.ReservationId && r.TrainerComment != null && r.TrainerRating != null)
                .FirstOrDefaultAsync();

            var review = await GetReviewByReservationIdClientId(clientReview.ReservationId, clientReview.ClientId);
            if (review == null)
            {
                throw new InvalidOperationException("Review not found.");
            }
            review.ClientComment = clientReview.ClientComment;
            review.ClientRating = clientReview.ClientRating;
            review.TrainerComment = existingTrainerReview?.TrainerComment;
            review.TrainerRating = existingTrainerReview?.TrainerRating;

            var result = await _context.Reviews.ReplaceOneAsync(r => r.Id == review.Id, _mapper.Map<Review>(review));
            return result.IsAcknowledged && result.ModifiedCount > 0;
        }
        
        public async Task<bool> SubmitTrainerReview(SubmitReviewDTO trainerReview)
        {
            var reviews = await _context.Reviews.Find(r => r.ReservationId == trainerReview.ReservationId).ToListAsync();
            if (reviews.Count == 0)
            {
                throw new InvalidOperationException("Review not found.");
            }

            var result = true;
            foreach (Review review in reviews)
            {
                review.TrainerComment = trainerReview.TrainerComment;
                review.TrainerRating = trainerReview.TrainerRating;
                var reviewResult = await _context.Reviews.ReplaceOneAsync(r => r.Id == review.Id, review);
                result = result && reviewResult.IsAcknowledged && reviewResult.ModifiedCount > 0;
            }
            return result;
        }
    }
}
