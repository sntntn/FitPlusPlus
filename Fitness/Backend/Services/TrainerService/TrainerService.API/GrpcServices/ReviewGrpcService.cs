using ReviewService.GRPC.Protos;

namespace TrainerService.API.GrpcServices
{
    public class ReviewGrpcService
    {
        private readonly ReviewProtoService.ReviewProtoServiceClient _reviewProtoServiceClient;

        public ReviewGrpcService(ReviewProtoService.ReviewProtoServiceClient reviewProtoServiceClient)
        {
            _reviewProtoServiceClient = reviewProtoServiceClient ?? throw new ArgumentNullException(nameof(reviewProtoServiceClient));
        }

        public async Task<GetReviewsResponse> GetReviews(string trainerId)
        {
            var getReviewsRequest = new GetReviewsRequest();
            getReviewsRequest.TrainerId = trainerId;
            return await _reviewProtoServiceClient.GetReviewsAsync(getReviewsRequest);
        }
    }
}
