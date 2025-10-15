using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IMongoCollection<Food> _foods;

        public FoodController(IMongoDatabase db)
        {
            _foods = db.GetCollection<Food>("Food");
        }
        
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost]
        public async Task<IActionResult> AddFood([FromBody] Food food)
        {
            if (string.IsNullOrWhiteSpace(food.Name))
                return BadRequest("Name is required");

            await _foods.InsertOneAsync(food);
            return Ok(food);
        }
        
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _foods.Find(_ => true).ToListAsync();
            return Ok(foods);
        }
    }
}
