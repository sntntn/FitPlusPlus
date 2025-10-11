using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainerService.Common.Entities
{
    public class ReviewType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TrainerId { get; set; }
        public string ClientId { get; set; }
        public string ClientComment { get; set; }
        public int ClientRating { get; set; }
    }
}
