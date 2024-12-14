using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace videoTrainingService.API.Entities
{
    public class TrainingExercise
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TrainingId { get; set; }

        public string ExerciseId { get; set; }
        public int ExerciseReps { get; set; }
        public int Set { get; set; }
        public int SetReps { get; set; }
    }
}