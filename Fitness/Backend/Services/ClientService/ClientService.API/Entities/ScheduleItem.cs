using MongoDB.Bson.Serialization.Attributes;

namespace ClientService.API.Entities
{
    public class ScheduleItem
    {
        [BsonIgnore]
        public TimeSpan StartTime { get; set; }
        [BsonIgnore]
        public TimeSpan EndTime { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public string TrainingType { get; set; }
        public bool IsAvailable { get; set; }
        public string TrainerId { get; set; }
        public string TrainerName { get; set; }

        public ScheduleItem(TimeSpan startTime, TimeSpan endTime,bool isAvailable)
        {
            StartTime = startTime;
            EndTime = endTime;
            StartHour = startTime.Hours;
            StartMinute = startTime.Minutes;
            EndHour = endTime.Hours;
            EndMinute = endTime.Minutes;
            IsAvailable = isAvailable;
            TrainingType = "";
            TrainerId = "";
            TrainerName = "";
        }
    }
}
