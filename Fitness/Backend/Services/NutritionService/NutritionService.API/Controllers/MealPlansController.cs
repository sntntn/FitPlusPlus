using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    /// <summary>
    /// Provides REST API endpoints for creating, retrieving, and deleting meal plans for clients based on their goals.
    /// </summary>
    /// <remarks>
    /// Trainers can create personalized meal plans for different goal types (lose, gain, maintain),
    /// while admins, trainers, and clients can retrieve existing plans.
    /// Each meal plan consists of predefined meals (breakfast, lunch, dinner, snacks) composed of available foods.
    /// </remarks>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MealPlansController : ControllerBase
    {
        private readonly IMongoCollection<MealPlan> _plans;
        private readonly IMongoCollection<Food> _foods;

        /// <summary>
        /// Initializes a new instance of the <see cref="MealPlansController"/> class.
        /// </summary>
        /// <param name="db">The MongoDB database instance used to access the <c>MealPlans</c> and <c>Food</c> collections.</param>
        public MealPlansController(IMongoDatabase db)
        {
            _plans = db.GetCollection<MealPlan>("MealPlans");
            _foods = db.GetCollection<Food>("Food");
        }

        /// <summary>
        /// Creates or updates a meal plan for a specific trainer and goal type.
        /// </summary>
        /// <param name="plan">The <see cref="MealPlan"/> object containing meals and trainer information.</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — if the plan was successfully created or updated.</description></item>
        /// <item><description><c>400 Bad Request</c> — if required fields (TrainerId, TrainerName, or GoalType) are missing.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Meal plan created or updated successfully.</response>
        /// <response code="400">Required data missing or invalid.</response>
        [Authorize(Roles = "Trainer")]
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

        /// <summary>
        /// Retrieves a specific meal plan created by a trainer for a given goal type.
        /// </summary>
        /// <param name="trainerId">The unique identifier of the trainer.</param>
        /// <param name="goalType">The goal type associated with the plan (lose, gain, maintain).</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — the corresponding meal plan was found.</description></item>
        /// <item><description><c>404 Not Found</c> — no plan exists for the specified trainer and goal type.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Meal plan successfully retrieved.</response>
        /// <response code="404">No matching plan found.</response>
        [Authorize(Roles = "Admin, Trainer, Client")]
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

        /// <summary>
        /// Retrieves all meal plans available in the system.
        /// </summary>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — all existing meal plans retrieved successfully.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">List of all meal plans retrieved.</response>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet]
        public async Task<IActionResult> GetAllPlans()
        {
            var plans = await _plans.Find(_ => true).ToListAsync();
            return Ok(plans);
        }

        /// <summary>
        /// Retrieves all meal plans associated with a specific trainer.
        /// </summary>
        /// <param name="trainerId">The unique identifier of the trainer.</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — list of all plans created by the specified trainer.</description></item>
        /// <item><description><c>404 Not Found</c> — no plans found for the trainer.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Trainer’s plans retrieved successfully.</response>
        /// <response code="404">No plans found for the specified trainer.</response>
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet("trainer/{trainerId}")]
        public async Task<IActionResult> GetPlansForTrainer(string trainerId)
        {
            var trainerPlans = await _plans.Find(p => p.TrainerId == trainerId).ToListAsync();

            if (!trainerPlans.Any())
                return NotFound(new { message = "No plans found for this trainer." });

            return Ok(trainerPlans);
        }

        /// <summary>
        /// Deletes a specific meal plan for a trainer and goal type.
        /// </summary>
        /// <param name="trainerId">The unique identifier of the trainer.</param>
        /// <param name="goalType">The goal type of the plan to be deleted.</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — if the plan was deleted successfully.</description></item>
        /// <item><description><c>404 Not Found</c> — if no plan was found for the specified trainer and goal type.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Plan deleted successfully.</response>
        /// <response code="404">No plan found for the given trainer and goal type.</response>
        [Authorize(Roles = "Trainer")]
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
