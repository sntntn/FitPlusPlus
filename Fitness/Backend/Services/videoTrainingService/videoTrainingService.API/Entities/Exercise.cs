using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace videoTrainingService.API.Entities
{
    public class Exercise
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string TrainerId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}