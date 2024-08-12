using AutoMapper;
using MongoDB.Driver;
using ReviewService.Common.Data;
using ReviewService.Common.DTOs;
using ReviewService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ReviewDTO>> GetReviews(string trainerId)
        {
            var reviews = await _context.Reviews.Find(r => r.TrainerId == trainerId).ToListAsync();
            return _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
        }

        public async Task CreateReview(CreateReviewDTO review)
        {
            var reviewEntity = _mapper.Map<Review>(review);
            await _context.Reviews.InsertOneAsync(reviewEntity);
        }

        public async Task<bool> DeleteReview(string reviewId)
        {
            var deleteResult = await _context.Reviews.DeleteManyAsync(r => r.Id == reviewId);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;

        }

        public async Task<bool> UpdateReview(UpdateReviewDTO review)
        {
            var reviewEntity = _mapper.Map<Review>(review);


            var updateResult = await _context.Reviews.ReplaceOneAsync(p => p.Id == reviewEntity.Id, reviewEntity);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
