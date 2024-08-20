namespace ClientService.API.Entities
{
    public class WeeklySchedule
    {
        public int WeekId { get; set; }
        public Dictionary<string, List<ScheduleItem>> DailySchedules { get; set; } = new Dictionary<string, List<ScheduleItem>>();

        private static readonly Dictionary<int, string> DayName = new Dictionary<int, string>
        {
            { 1, "Monday" },
            { 2, "Tuesday" },
            { 3, "Wednesday" },
            { 4, "Thursday" },
            { 5, "Friday" },
            { 6, "Saturday" },
            { 7, "Sunday" }
        };
        public WeeklySchedule(int weekId)
        {
            WeekId = weekId;

            for (int i = 1; i <= 7; ++i)
            {
                DailySchedules[DayName[i]] = InitializeDay();
            }
        }

        private static List<ScheduleItem> InitializeDay()
        {
            var timeslots = new List<ScheduleItem>();
            var startTime = new TimeSpan(8, 0, 0); // 8:00 AM
            var endTime = new TimeSpan(20, 0, 0); // 8:00 PM
            
            while (startTime < endTime)
            {
                var nextTime = startTime.Add(TimeSpan.FromMinutes(15));
                timeslots.Add(new ScheduleItem(startTime,nextTime,true));
                startTime = nextTime;
            }
            return timeslots;
        }
    }
}
