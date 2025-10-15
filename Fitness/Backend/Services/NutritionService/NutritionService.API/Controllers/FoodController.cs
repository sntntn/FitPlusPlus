using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    /// <summary>
    /// Provides REST API endpoints for managing food items in the nutrition database.
    /// </summary>
    /// <remarks>
    /// This controller supports CRUD operations for foods, allowing administrators and trainers
    /// to add new food entries, and all authorized users (including clients) to view the list of foods.
    /// 
    /// The controller interacts directly with a MongoDB collection named <c>Food</c>.
    /// </remarks>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IMongoCollection<Food> _foods;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoodController"/> class.
        /// </summary>
        /// <param name="db">The MongoDB database instance used to access the <c>Food</c> collection.</param>
        public FoodController(IMongoDatabase db)
        {
            _foods = db.GetCollection<Food>("Food");
        }

        /// <summary>
        /// Adds a new food item to the database.
        /// </summary>
        /// <param name="food">The <see cref="Food"/> object to be inserted into the database.</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — if the food item was successfully added.</description></item>
        /// <item><description><c>400 Bad Request</c> — if the food name is missing or invalid.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">The food item was successfully added to the database.</response>
        /// <response code="400">Invalid input — the food name is required.</response>
        [Authorize(Roles = "Admin, Trainer")]
        [HttpPost]
        public async Task<IActionResult> AddFood([FromBody] Food food)
        {
            if (string.IsNullOrWhiteSpace(food.Name))
                return BadRequest("Name is required");

            await _foods.InsertOneAsync(food);
            return Ok(food);
        }

        /// <summary>
        /// Retrieves all available food items from the database.
        /// </summary>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — a list of all food items currently stored in the database.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">List of food items successfully retrieved.</response>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet]
        public async Task<IActionResult> GetAllFoods()
        {
            var foods = await _foods.Find(_ => true).ToListAsync();
            return Ok(foods);
        }
    }
}
