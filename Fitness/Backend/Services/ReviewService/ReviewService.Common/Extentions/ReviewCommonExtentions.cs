using Microsoft.Extensions.DependencyInjection;
using ReviewService.Common.Data;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;
using ReviewService.Common.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Common.Extentions
{
    public static class ReviewCommonExtentions
    {
        public static void AddReviewCommonExtentions(this IServiceCollection services)
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
