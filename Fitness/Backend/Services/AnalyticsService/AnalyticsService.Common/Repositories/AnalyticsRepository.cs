using AnalyticsService.Common.Data;
using AnalyticsService.Common.DTOs;
using AnalyticsService.Common.Entities;
using AutoMapper;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace AnalyticsService.Common.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly IAnalyticsContext _context;
    private readonly IMapper _mapper;

    public AnalyticsRepository(IAnalyticsContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task CreateTraining(Training training)
    {
        await _context.Trainings.InsertOneAsync(training);
    }

    public async Task<bool> DeleteTraining(string id)
    {
        var deleteResult = await _context.Trainings.DeleteOneAsync(t => t.Id == id);
        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }

    public async Task<double> GetTrainerAverageTrainingRating(string trainerId)
    {
        var trainings = await _context.Trainings.Find(t => t.TrainerId == trainerId).ToListAsync();
        return trainings.Count > 0 ? trainings.Average(t => t.ClientRating) : 0;
    }

    public async Task<int> GetTrainerNumOfTrainings(string trainerId)
    {
       long numOfElements = await _context.Trainings
           .Find(t => t.TrainerId == trainerId && t.Status == TrainingStatus.HELD)
           .CountDocumentsAsync();
       
       return Convert.ToInt32(numOfElements);
    }

    public async Task<int> GetClientNumOfTrainings(string clientId)
    {
        long numOfElements = await _context.Trainings
            .Find(t => t.ClientId == clientId && t.Status == TrainingStatus.HELD)
            .CountDocumentsAsync();
        
        return Convert.ToInt32(numOfElements);
    }
    
    public async Task<int> GetClientNumOfHeldTrainings(string clientId)
    {
        long numOfElements = await _context.Trainings
            .Find(t => t.Status == TrainingStatus.HELD)
            .CountDocumentsAsync();
        return Convert.ToInt32(numOfElements);
    }
    
    public async Task<int> GetClientNumOfCancelledTrainings(string clientId)
    {
        long numOfElements = await _context.Trainings
            .Find(t => t.Status == TrainingStatus.CANCELLED)
            .CountDocumentsAsync();
        return Convert.ToInt32(numOfElements);
    }

    public async Task<IEnumerable<string>> GetTrainerClientIds(string trainerId)
    {
        var trainings = await _context.Trainings.FindAsync(t => t.TrainerId == trainerId);
        return trainings.ToList().Select(t => t.ClientId).AsEnumerable();
    }

    public async Task<IEnumerable<ClientTrainingDTO>> GetClientTrainings(string clientId)
    {
        var clientTrainings = await _context.Trainings.FindAsync(t => t.ClientId == clientId);
        return _mapper.Map<IEnumerable<ClientTrainingDTO>>(clientTrainings.ToList());
    }
}