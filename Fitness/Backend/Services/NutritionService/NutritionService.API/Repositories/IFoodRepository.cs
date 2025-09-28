using NutritionService.API.Models;

namespace NutritionService.API.Repositories
{
    public interface IFoodRepository
    {
        Task<IEnumerable<Food>> GetAllAsync();
        Task<Food?> GetByIdAsync(string id);
        Task CreateAsync(Food food);
        Task UpdateAsync(Food food);
        Task DeleteAsync(string id);
    }
}
