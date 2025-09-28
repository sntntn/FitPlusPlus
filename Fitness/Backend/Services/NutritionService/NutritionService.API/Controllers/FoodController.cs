using Microsoft.AspNetCore.Mvc;
using NutritionService.API.Models;
using NutritionService.API.Repositories;

namespace NutritionService.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FoodController : ControllerBase
    {
        private readonly IFoodRepository _repository;

        public FoodController(IFoodRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _repository.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create(Food food)
        {
            await _repository.CreateAsync(food);
            return CreatedAtAction(nameof(GetAll), food);
        }
    }
}
