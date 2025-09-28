using Microsoft.AspNetCore.Mvc;
using NutritionService.API.Models;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GoalsController : ControllerBase
    {
        [HttpPost]
        public IActionResult SetGoal([FromBody] UserGoal goal)
        {
            int baseline = 2000;
            int adjustment = 0;

            if (goal.TargetWeight < goal.CurrentWeight)
            {
                adjustment = goal.Pace switch
                {
                    "slow" => -300,
                    "medium" => -500,
                    "fast" => -700,
                    _ => -500
                };
            }
            else if (goal.TargetWeight > goal.CurrentWeight)
            {
                adjustment = goal.Pace switch
                {
                    "slow" => +200,
                    "medium" => +300,
                    "fast" => +500,
                    _ => +300
                };
            }

            goal.TargetKcal = baseline + adjustment;
            return Ok(goal);
        }
    }
}
