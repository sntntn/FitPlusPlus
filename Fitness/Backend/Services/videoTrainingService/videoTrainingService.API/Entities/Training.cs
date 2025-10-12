using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace videoTrainingService.API.Entities
{
    public class Training
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TrainingId { get; set; }
        public string TrainerId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> ClientIds { get; set;}
    }
}