using ReviewService.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Common.Repositories
{
    public interface IReviewRepository
    {
        Task<ReviewDTO?> GetReviewByReservationId(string reservationId);
        Task<IEnumerable<ReviewDTO>> GetReviewsByTrainerId(string trainerId);
        Task<IEnumerable<ReviewDTO>> GetReviewsByClientId(string clientId);
        Task CreateReview(string reservationId, string clientId, string trainerId);
        Task<bool> SubmitClientReview(SubmitReviewDTO clientReview);
        Task<bool> SubmitTrainerReview(SubmitReviewDTO trainerReview);
    }
}
