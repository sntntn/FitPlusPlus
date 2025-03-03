using Microsoft.Extensions.DependencyInjection;
using ReviewService.Common.Data;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;
using ReviewService.Common.Repositories;

namespace ReviewService.Common.Extensions
{
    public static class ReviewCommonExtensions
    {
        public static void AddReviewCommonExtensions(this IServiceCollection services)
        {
            services.AddScoped<IReviewContext, ReviewContext>();
            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddAutoMapper(configuration =>
            {
                configuration.CreateMap<ReviewDTO, Review>().ReverseMap();
                configuration.CreateMap<CreateReviewDTO, Review>().ReverseMap();
                configuration.CreateMap<UpdateReviewDTO, Review>().ReverseMap();
            });
        }
    }
}
