using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainerService.API.Entities
{
    public class ReviewType
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TrainerId { get; set; }
        public string ClientId { get; set; }
        public string Comment { get; set; }
        public int Rating { get; set; }
    }
}
