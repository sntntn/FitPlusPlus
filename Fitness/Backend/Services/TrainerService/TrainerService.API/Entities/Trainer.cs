using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainerService.API.Entities
{
    public class Trainer
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string FullName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Bio { get; set; }
        public List<TrainingType> TrainingTypes { get; set; } = new List<TrainingType>();



    }
}
