using MongoDB.Driver;
using NutritionService.API.Models;

namespace NutritionService.API.Repositories
{
    public class FoodRepository : IFoodRepository
    {
        private readonly IMongoCollection<Food> _foods;

        public FoodRepository(IConfiguration config)
        {
            var client = new MongoClient(config.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(config.GetValue<string>("DatabaseSettings:DatabaseName"));
            _foods = database.GetCollection<Food>("Foods");
        }

        public async Task<IEnumerable<Food>> GetAllAsync() =>
            await _foods.Find(f => true).ToListAsync();

        public async Task<Food?> GetByIdAsync(string id) =>
            await _foods.Find(f => f.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Food food) =>
            await _foods.InsertOneAsync(food);

        public async Task UpdateAsync(Food food) =>
            await _foods.ReplaceOneAsync(f => f.Id == food.Id, food);

        public async Task DeleteAsync(string id) =>
            await _foods.DeleteOneAsync(f => f.Id == id);
    }
}
