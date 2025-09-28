namespace NutritionService.API.Models
{
    public class UserGoal
    {
        public string? Id { get; set; }          
        public string ClientId { get; set; }
        public double CurrentWeight { get; set; }
        public double TargetWeight { get; set; }
        public string Pace { get; set; }         
        public int TargetKcal { get; set; }
    }
}

