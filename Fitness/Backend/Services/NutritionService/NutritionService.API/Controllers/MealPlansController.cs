using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MealPlansController : ControllerBase
    {
        private readonly IMongoCollection<MealPlan> _plans;
        private readonly IMongoCollection<Food> _foods;

        public MealPlansController(IMongoDatabase db)
        {
            _plans = db.GetCollection<MealPlan>("MealPlans");
            _foods = db.GetCollection<Food>("Food");
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] MealPlan plan)
        {
            if (string.IsNullOrEmpty(plan.TrainerId) || string.IsNullOrEmpty(plan.TrainerName))
                return BadRequest(new { message = "TrainerId and TrainerName are required." });

            if (string.IsNullOrEmpty(plan.GoalType))
                return BadRequest(new { message = "GoalType is required." });

            async Task<List<Food>> FillFoodsAsync(List<Food> foods)
            {
                var filled = new List<Food>();
                foreach (var f in foods)
                {
                    var dbFood = await _foods.Find(x => x.Name == f.Name).FirstOrDefaultAsync();
                    if (dbFood != null)
                        filled.Add(dbFood);
                }
                return filled;
            }

            plan.Breakfast = await FillFoodsAsync(plan.Breakfast);
            plan.Lunch = await FillFoodsAsync(plan.Lunch);
            plan.Dinner = await FillFoodsAsync(plan.Dinner);
            plan.Snacks = await FillFoodsAsync(plan.Snacks);
            plan.CreatedAt = DateTime.UtcNow;

            
            var existing = await _plans
                .Find(p => p.TrainerId == plan.TrainerId && p.GoalType == plan.GoalType)
                .FirstOrDefaultAsync();

            if (existing != null)
            {
                await _plans.DeleteOneAsync(p => p.Id == existing.Id);
            }

            await _plans.InsertOneAsync(plan);
            return Ok(plan);
        }

        [HttpGet("trainer/{trainerId}/goal/{goalType}")]
        public async Task<IActionResult> GetPlanByTrainerAndGoal(string trainerId, string goalType)
        {
            var plan = await _plans
                .Find(p => p.TrainerId == trainerId && p.GoalType == goalType)
                .SortByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            if (plan == null)
                return NotFound(new { message = "No plan found for this trainer and goal type." });

            return Ok(plan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _plans.Find(_ => true).ToListAsync();
            return Ok(plans);
        }

        [HttpGet("trainer/{trainerId}")]
        public async Task<IActionResult> GetPlansForTrainer(string trainerId)
        {
            var trainerPlans = await _plans.Find(p => p.TrainerId == trainerId).ToListAsync();

            if (!trainerPlans.Any())
                return NotFound(new { message = "No plans found for this trainer." });

            return Ok(trainerPlans);
        }

        [HttpDelete("trainer/{trainerId}/goal/{goalType}")]
        public async Task<IActionResult> DeletePlan(string trainerId, string goalType)
        {
            var result = await _plans.DeleteOneAsync(p =>
                p.TrainerId == trainerId && p.GoalType == goalType
            );

            if (result.DeletedCount == 0)
                return NotFound(
                    new
                    {
                        message = $"No plan found for trainer '{trainerId}' and goal '{goalType}'.",
                    }
                );

            return Ok(
                new
                {
                    message = $"Plan for trainer '{trainerId}' and goal '{goalType}' deleted successfully.",
                }
            );
        }
    }
}
