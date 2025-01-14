using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ClientService.Common.Entities
{
    public class ScheduleItem
    {
 
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public string TrainingType { get; set; }
        public bool IsAvailable { get; set; }
        public string TrainerId { get; set; }
        public string TrainerName { get; set; }
        public int TrainingStartHour { get; set; }
        public int TrainingStartMinute { get; set; }
        public TimeSpan TrainingDuration {  get; set; }

        public ScheduleItem(TimeSpan startTime, TimeSpan endTime,bool isAvailable)
        {

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
