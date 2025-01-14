namespace ClientService.Common.Entities
{
    public class BookTrainingInformation
    {
        public string ClientId { get; set; }
        public string TrainerName { get; set; }
        public string TrainerId { get; set; }
        public string TrainingType { get; set; }
        public TimeSpan Duration { get; set; }
        public int WeekId { get; set; }
        public string DayName { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public bool IsBooking { get; set; }
    }
}
