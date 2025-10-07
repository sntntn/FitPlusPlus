using Microsoft.AspNetCore.Mvc;
using NutritionService.API.Models;
using MongoDB.Driver;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IMongoCollection<UserGoal> _goals;

        public GoalsController(IMongoDatabase db)
        {
            _goals = db.GetCollection<UserGoal>("Goals");
        }
        [HttpPost]
        public  async Task<IActionResult> SetGoal([FromBody] UserGoal goal)
        {
            // 1) BMR (Mifflin–St Jeor sa interneta formula)
            int bmr = goal.Sex == "male"
                ? (int)(10 * goal.CurrentWeight + 6.25 * goal.Height - 5 * goal.Age + 5)
                : (int)(10 * goal.CurrentWeight + 6.25 * goal.Height - 5 * goal.Age - 161);

            // 2) Aktivnost (TDEE)
            double activityFactor = goal.ActivityLevel switch
            {
                "sedentary"  => 1.2,
                "light"      => 1.375,
                "moderate"   => 1.55,
                "active"     => 1.725,
                "veryActive" => 1.9,
                _ => 1.2
            };
            int tdee = (int)(bmr * activityFactor);

            // 3) Intensity
            int adjust = goal.Intensity switch
            {
                "low"    => 300,
                "medium" => 500,
                "high"   => 700,
                _ => 500
            };

            // 4) GoalType
            goal.TargetKcal = goal.GoalType switch
            {
                "lose"     => Math.Max(1200, tdee - adjust),
                "gain"     => tdee + adjust,
                "maintain" => tdee,
                _          => tdee
            };

            // 5) BMI
            var h = goal.Height / 100.0;
            goal.BMI = Math.Round(goal.CurrentWeight / (h * h), 2);

            await _goals.InsertOneAsync(goal);
            return Ok(goal);
        }

        [HttpGet("plan/{clientId}")]
        public async Task<IActionResult> GetPlan(string clientId)
        {
            var goal = await _goals
                .Find(g => g.ClientId == clientId)
                .SortByDescending(g => g.Id)
                .FirstOrDefaultAsync();

            if (goal == null) return NotFound("No goal found for this client.");
            return Ok(goal);
        }


        
    }
}
