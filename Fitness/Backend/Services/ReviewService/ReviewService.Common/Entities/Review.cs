using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewService.Common.Entities
{
    public class Review
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id;
        public string ReservationId { get; set; }
        public string TrainerId { get; set; }
        public string? TrainerComment { get; set; }
        public int? TrainerRating { get; set; }
        public string ClientId { get; set; }
        public string? ClientComment { get; set; }
        public int? ClientRating { get; set; }
    }
}
