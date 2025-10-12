using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NutritionService.API.Models;


namespace NutritionService.API.Models
{
    public class MealPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string GoalType { get; set; }
        public List<Food> Breakfast { get; set; } = new();
        public List<Food> Lunch { get; set; } = new();
        public List<Food> Dinner { get; set; } = new();
        public List<Food> Snacks { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}




