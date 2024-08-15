using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ReviewService.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Common.Data
{
    public class ReviewContext : IReviewContext
    {
        public ReviewContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase("ReviewDB");

            Reviews = database.GetCollection<Review>("Reviews");
        }

        public IMongoCollection<Review> Reviews { get; }
    }
}
