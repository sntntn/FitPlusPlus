using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IMongoCollection<UserGoal> _goals;

        public GoalsController(IMongoDatabase db)
        {
            _goals = db.GetCollection<UserGoal>("Goals");
        }
        
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpPost]
        public async Task<IActionResult> SetGoal([FromBody] UserGoal goal)
        {
            int bmr =
                goal.Sex == "male"
                    ? (int)(10 * goal.CurrentWeight + 6.25 * goal.Height - 5 * goal.Age + 5)
                    : (int)(10 * goal.CurrentWeight + 6.25 * goal.Height - 5 * goal.Age - 161);

            double activityFactor = goal.ActivityLevel switch
            {
                "sedentary" => 1.2,
                "light" => 1.375,
                "moderate" => 1.55,
                "active" => 1.725,
                "veryActive" => 1.9,
                _ => 1.2,
            };
            int tdee = (int)(bmr * activityFactor);

            int adjust = goal.Intensity switch
            {
                "low" => 300,
                "medium" => 500,
                "high" => 700,
                _ => 500,
            };

            goal.TargetKcal = goal.GoalType switch
            {
                "lose" => Math.Max(1200, tdee - adjust),
                "gain" => tdee + adjust,
                "maintain" => tdee,
                _ => tdee,
            };

            var h = goal.Height / 100.0;
            goal.BMI = Math.Round(goal.CurrentWeight / (h * h), 2);

            if (string.IsNullOrWhiteSpace(goal.ClientId))
            {
                goal.ClientId = Guid.NewGuid().ToString();
            }

            await _goals.InsertOneAsync(goal);
            return Ok(goal);
        }
        
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("plan/{clientId}")]
        public async Task<IActionResult> GetPlan(string clientId)
        {
            var goal = await _goals
                .Find(g => g.ClientId == clientId)
                .SortByDescending(g => g.Id)
                .FirstOrDefaultAsync();

            if (goal == null)
                return NotFound("No goal found for this client.");
            return Ok(goal);
        }
        
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllGoals()
        {
            var allGoals = await _goals.Find(_ => true).ToListAsync();

            if (allGoals == null || allGoals.Count == 0)
                return NotFound("No goals found.");

            var simplified = allGoals.Select(g => new
            {
                g.ClientId,
                g.BMI,
                g.TargetKcal,
            });

            return Ok(simplified);
        }
    }
}
