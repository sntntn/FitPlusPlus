using System.ComponentModel.DataAnnotations;
using AnalyticsService.Common.Data;
using AnalyticsService.Common.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace AnalyticsService.Common.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly IAnalyticsContext _context;

    public AnalyticsRepository(IAnalyticsContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task CreateTraining(Training training)
    {
        await _context.Trainings.InsertOneAsync(training);
    }

    public async Task<double> GetAverageRating(string trainerId)
    {
        var trainings = await _context.Trainings.Find(t => t.TrainerId == trainerId).ToListAsync();
        return trainings.Count > 0 ? trainings.Average(t => t.Rating) : 0;
    }

    public async Task<int> GetTrainerNumOfTrainings(string trainerId)
    {
       long numOfElements = await _context.Trainings
           .Find(t => t.TrainerId == trainerId && t.Status == TrainingStatus.HELD)
           .CountDocumentsAsync();
       
       return Convert.ToInt32(numOfElements);
    }

    public async Task<int> GetClientNumOfTraining(string clientId)
    {
        long numOfElements = await _context.Trainings
            .Find(t => t.ClientId == clientId && t.Status == TrainingStatus.HELD)
            .CountDocumentsAsync();
        
        return Convert.ToInt32(numOfElements);
    }
}