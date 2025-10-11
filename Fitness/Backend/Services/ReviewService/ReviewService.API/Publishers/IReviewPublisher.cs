using ReviewService.Common.DTOs;

namespace ReviewService.API.Publishers;

public interface IReviewPublisher
{
    Task PublishReview(SubmitReviewDTO submittedReview, bool client);
}