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
        Task<IEnumerable<ReviewDTO>> GetReviews(string trainerId);
        Task CreateReview(CreateReviewDTO review);
        Task<bool> UpdateReview(UpdateReviewDTO review);
        Task<bool> DeleteReview(string reviewId);
    }
}
