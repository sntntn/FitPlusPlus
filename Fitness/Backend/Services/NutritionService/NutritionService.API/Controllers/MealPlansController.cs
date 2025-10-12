using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            await _plans.InsertOneAsync(plan);
            return Ok(plan);
        }

        [HttpGet("{goalType}")]
        public async Task<IActionResult> GetPlanByGoalType(string goalType)
        {
            var plan = await _plans
                .Find(p => p.GoalType == goalType)
                .SortByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();

            if (plan == null)
                return NotFound(new { message = "No plan found for this goal type." });

            return Ok(plan);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _plans.Find(_ => true).ToListAsync();
            return Ok(plans);
        }

        [HttpDelete("{goalType}")]
        public async Task<IActionResult> DeletePlan(string goalType)
        {
            var result = await _plans.DeleteOneAsync(p => p.GoalType == goalType);

            if (result.DeletedCount == 0)
                return NotFound(new { message = $"No plan found for goal type '{goalType}'." });

            return Ok(new { message = $"Plan for goal type '{goalType}' deleted successfully." });
        }
    }
}
