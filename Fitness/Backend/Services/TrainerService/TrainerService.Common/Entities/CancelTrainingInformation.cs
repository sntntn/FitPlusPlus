namespace TrainerService.Common.Entities
{
    public class CancelTrainingInformation
    {
        public string ClientId { get; set; }
        public string TrainerId { get; set; }
        public TimeSpan Duration { get; set; }
        public int WeekId { get; set; }
        public string DayName { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
    }
}
