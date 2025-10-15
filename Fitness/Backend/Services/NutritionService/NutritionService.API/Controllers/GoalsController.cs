using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    /// <summary>
    /// Provides REST API endpoints for setting and retrieving client nutrition and fitness goals.
    /// </summary>
    /// <remarks>
    /// This controller handles client-specific goal calculations based on biometric data,
    /// activity levels, and desired goal types (lose, gain, maintain).  
    /// It interacts with the MongoDB <c>Goals</c> collection and calculates BMR, TDEE, BMI, and target calories.
    /// </remarks>
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class GoalsController : ControllerBase
    {
        private readonly IMongoCollection<UserGoal> _goals;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoalsController"/> class.
        /// </summary>
        /// <param name="db">The MongoDB database instance used to access the <c>Goals</c> collection.</param>
        public GoalsController(IMongoDatabase db)
        {
            _goals = db.GetCollection<UserGoal>("Goals");
        }

        /// <summary>
        /// Creates and stores a new goal for a client based on biometric and lifestyle data.
        /// </summary>
        /// <param name="goal">The <see cref="UserGoal"/> object containing the client's details and preferences.</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — if the goal was calculated and saved successfully.</description></item>
        /// <item><description><c>400 Bad Request</c> — if the goal data is incomplete or invalid.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Goal successfully created and saved.</response>
        /// <response code="400">Invalid or missing goal data.</response>
        [Authorize(Roles = "Client")]
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

        /// <summary>
        /// Retrieves the most recent goal plan for a specific client.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — if a goal was found and returned successfully.</description></item>
        /// <item><description><c>404 Not Found</c> — if no goal exists for the specified client.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">Latest goal retrieved successfully.</response>
        /// <response code="404">No goal found for the specified client.</response>
        [Authorize(Roles = "Client")]
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

        /// <summary>
        /// Retrieves all goals stored in the database (admin or trainer overview).
        /// </summary>
        /// <returns>
        /// Returns:
        /// <list type="bullet">
        /// <item><description><c>200 OK</c> — list of all goals in simplified form (ClientId, BMI, TargetKcal).</description></item>
        /// <item><description><c>404 Not Found</c> — if there are no stored goals.</description></item>
        /// </list>
        /// </returns>
        /// <response code="200">List of goals retrieved successfully.</response>
        /// <response code="404">No goals found in the database.</response>
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
