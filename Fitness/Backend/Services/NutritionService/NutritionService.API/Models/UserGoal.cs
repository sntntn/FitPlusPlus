using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NutritionService.API.Models
{
    public class UserGoal
    
    {
        [BsonId] 
        [BsonRepresentation(BsonType.ObjectId)] 
        public string? Id { get; set; }          
        public string ClientId { get; set; }
                // Podaci koje klijent unosi
        public string Sex { get; set; }     
        public int Age { get; set; }
        public double Height { get; set; }  
        public double CurrentWeight { get; set; }
        public string ActivityLevel { get; set; } 
        public string GoalType { get; set; }      
        public string Intensity {get; set; }

        
        public int TargetKcal { get; set; }
        public double BMI { get; set; }

}
}

