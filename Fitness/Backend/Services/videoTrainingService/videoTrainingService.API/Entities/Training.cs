using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace videoTrainingService.API.Entities
{
    public class Training
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TrainingId { get; set; }

        public string ClientId { get; set; }
        public string TrainerId { get; set; }
    }
}