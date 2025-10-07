using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MealPlanController : ControllerBase
    {
        private readonly IMongoCollection<MealPlan> _plans;

        public MealPlanController(IMongoDatabase db)
        {
            _plans = db.GetCollection<MealPlan>("MealPlans");
        }

       
        [HttpPost]
        public async Task<IActionResult> CreatePlan([FromBody] MealPlan plan)
        {
            await _plans.InsertOneAsync(plan);
            return Ok(plan);
        }

        
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetPlan(string clientId)
        {
            var plan = await _plans.Find(p => p.ClientId == clientId)
                                   .SortByDescending(p => p.CreatedAt)
                                   .FirstOrDefaultAsync();

            if (plan == null)
                return NotFound(new { message = "Meal plan not created yet." });

            return Ok(plan);
        }
    }
}

