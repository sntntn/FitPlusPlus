using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TrainerService.API.Entities
{
    public class TrainerSchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("TrainerId")]
        public string TrainerId { get; set; }
        public List<WeeklySchedule> WeeklySchedules { get; set; } = new List<WeeklySchedule>();
        public TrainerSchedule(string id)
        {
            var startWeek = 1;
            var endWeek = 3;
            TrainerId = id;
            for (int i = startWeek; i <= endWeek; i++)
            {
                WeeklySchedules.Add(new WeeklySchedule(i));
            }
        }
    }
}
