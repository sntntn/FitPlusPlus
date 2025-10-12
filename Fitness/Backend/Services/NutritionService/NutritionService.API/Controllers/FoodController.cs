using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IMongoCollection<Food> _foods;

        public FoodController(IMongoDatabase db)
        {
            _foods = db.GetCollection<Food>("Food");
        }

        [HttpPost]
        public async Task<IActionResult> AddFood([FromBody] Food food)
        {
            if (string.IsNullOrWhiteSpace(food.Name))
                return BadRequest("Name is required");

            await _foods.InsertOneAsync(food);
            return Ok(food);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _foods.Find(_ => true).ToListAsync();
            return Ok(foods);
        }
    }
}
