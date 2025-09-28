namespace NutritionService.API.Models
{
    public class Food
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public double Protein { get; set; }
        public double Carbs { get; set; }
        public double Fat { get; set; }
    }
}

