using AnalyticsService.Common.Entities;
using AutoMapper;
using ReviewService.GRPC.Protos;

namespace AnalyticsService.API.Mapper;

public class ReviewResponseProfile : Profile
{
    public ReviewResponseProfile()
    {
        CreateMap<GetReviewsResponse, ReviewType>().ReverseMap();
        CreateMap<GetReviewsResponse.Types.ReviewReply, ReviewType>().ReverseMap();
    }
}
