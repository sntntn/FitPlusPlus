using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionService.API.Models
{
    public class MealPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string TrainerId { get; set; } = string.Empty;
        public string TrainerName { get; set; } = string.Empty;
        public string GoalType { get; set; } = string.Empty;

        public List<Food> Breakfast { get; set; } = new();
        public List<Food> Lunch { get; set; } = new();
        public List<Food> Dinner { get; set; } = new();
        public List<Food> Snacks { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}




