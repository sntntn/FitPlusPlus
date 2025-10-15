using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NutritionService.API.Controllers
{   
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HealthController : ControllerBase
    {
        [Authorize(Roles = "Admin, Trainer, Client")]
        [HttpGet]
        public IActionResult Get() => Ok("NutritionService running..");
    }
}
