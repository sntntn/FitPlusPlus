using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionService.API.Models
{
    public class MealPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string ClientId { get; set; } 
        public string TrainerName { get; set; } 

        public List<string> Breakfast { get; set; } = new();
        public List<string> Lunch { get; set; } = new();
        public List<string> Dinner { get; set; } = new();
        public List<string> Snacks { get; set; } = new();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

