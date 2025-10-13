using Microsoft.AspNetCore.Mvc;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get() => Ok("NutritionService running..");
    }
}
