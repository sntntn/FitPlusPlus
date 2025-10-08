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
        public string TrainerComment { get; set; }
        public string TrainerRating { get; set; }
        public string ClientId { get; set; }
        public string ClientComment { get; set; }
        public int ClientRating { get; set; }
    }
    
    /*
     * When creating a Review, the flow will be as follows:
     *  - If there is not a Review entity created for the Reservation,
     *    which is being reviewed, create one. Otherwise, fetch that
     *    Review.
     *  - Update the Review entity with the new data.
     */
}
