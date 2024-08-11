using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Common.Entities;

namespace ReviewService.Common.Data
{
    public interface IReviewContext
    {
        IMongoCollection<Review> Reviews { get; }
    }
}
