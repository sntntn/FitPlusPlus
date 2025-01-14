using AutoMapper;
using Grpc.Core;
using ReviewService.Common.Repositories;
using ReviewService.GRPC.Protos;
using static ReviewService.GRPC.Protos.GetReviewsResponse.Types;

namespace ReviewService.GRPC.Services

{
    public class ReviewServiceGrpc : ReviewProtoService.ReviewProtoServiceBase
    {
        private readonly IReviewRepository _repository;
        private readonly IMapper _mapper;


        public ReviewServiceGrpc(IReviewRepository repository, IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public override async Task<GetReviewsResponse> GetReviews(GetReviewsRequest request, ServerCallContext context)
        {
            var reviews = await _repository.GetReviews(request.TrainerId);
            var reviewList = new GetReviewsResponse();
            reviewList.Reviews.AddRange(_mapper.Map<IEnumerable<ReviewReply>>(reviews));
            return reviewList;
        }
    }
}
