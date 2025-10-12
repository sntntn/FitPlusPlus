using AnalyticsService.Common.Data;
using AnalyticsService.Common.Entities;
using AutoMapper;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace AnalyticsService.Common.Repositories;

public class AnalyticsRepository : IAnalyticsRepository
{
    private readonly IAnalyticsContext _context;
    private readonly IMapper _mapper;

    // Individual Trainings
    public AnalyticsRepository(IAnalyticsContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task CreateIndividualTraining(IndividualTraining individualTraining)
    {
        await _context.IndividualTrainings.InsertOneAsync(individualTraining);
    }

    public async Task<IndividualTraining> GetIndividualTrainingByReservationId(string reservationId)
    {
        return await _context.IndividualTrainings.Find(it => it.ReservationId == reservationId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<IndividualTraining>> GetIndividualTrainingsByTrainerId(string trainerId)
    {
        return await _context.IndividualTrainings.Find(it => it.TrainerId == trainerId).ToListAsync();
    }

    public async Task<IEnumerable<IndividualTraining>> GetIndividualTrainingsByClientId(string clientId)
    {
        return await _context.IndividualTrainings.Find(it => it.ClientId == clientId).ToListAsync();
    }

    public async Task<bool> UpdateIndividualTraining(IndividualTraining individualTraining)
    {
        var result = await _context.IndividualTrainings.ReplaceOneAsync(it => it.Id == individualTraining.Id, individualTraining);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteIndividualTraining(string id)
    {
        var result = await _context.IndividualTrainings.DeleteOneAsync(it => it.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task CreateGroupTraining(GroupTraining groupTraining)
    {
        await _context.GroupTrainings.InsertOneAsync(groupTraining);
    }

    // Group Trainings
    public async Task<IEnumerable<GroupTraining>> GetGroupTrainingsByTrainerId(string trainerId)
    {
        return await _context.GroupTrainings.Find(gt => gt.TrainerId == trainerId).ToListAsync();
    }

    public async Task<GroupTraining> GetGroupTrainingByReservationId(string reservationId)
    {
        return await _context.GroupTrainings.Find(gt => gt.ReservationId == reservationId).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GroupTraining>> GetGroupTrainingsByClientId(string clientId)
    {
        return await _context.GroupTrainings.Find(gt => gt.ClientIds.Contains(clientId)).ToListAsync();
    }

    public async Task<bool> UpdateGroupTraining(GroupTraining groupTraining)
    {
        var result = await _context.GroupTrainings.ReplaceOneAsync(gt => gt.Id == groupTraining.Id, groupTraining);
        return result.IsAcknowledged && result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteGroupTraining(string id)
    {
        var result = await _context.GroupTrainings.DeleteOneAsync(gt => gt.Id == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
    
    // public async Task CreateTraining(Training training)
    // {
    //     await _context.Trainings.InsertOneAsync(training);
    // }
    //
    // public async Task<bool> DeleteTraining(string id)
    // {
    //     var deleteResult = await _context.Trainings.DeleteOneAsync(t => t.Id == id);
    //     return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    // }
    //
    // public async Task<double> GetTrainerAverageTrainingRating(string trainerId)
    // {
    //     var trainings = await _context.Trainings.Find(t => t.TrainerId == trainerId).ToListAsync();
    //     return trainings.Count > 0 ? trainings.Average(t => t.ClientRating) : 0;
    // }
    //
    // public async Task<int> GetTrainerNumOfTrainings(string trainerId)
    // {
    //    long numOfElements = await _context.Trainings
    //        .Find(t => t.TrainerId == trainerId && t.Status == TrainingStatus.HELD)
    //        .CountDocumentsAsync();
    //    
    //    return Convert.ToInt32(numOfElements);
    // }
    //
    // public async Task<int> GetClientNumOfTrainings(string clientId)
    // {
    //     long numOfElements = await _context.Trainings
    //         .Find(t => t.ClientId == clientId && t.Status == TrainingStatus.HELD)
    //         .CountDocumentsAsync();
    //     
    //     return Convert.ToInt32(numOfElements);
    // }
    //
    // public async Task<int> GetClientNumOfHeldTrainings(string clientId)
    // {
    //     long numOfElements = await _context.Trainings
    //         .Find(t => t.Status == TrainingStatus.HELD)
    //         .CountDocumentsAsync();
    //     return Convert.ToInt32(numOfElements);
    // }
    //
    // public async Task<int> GetClientNumOfCancelledTrainings(string clientId)
    // {
    //     long numOfElements = await _context.Trainings
    //         .Find(t => t.Status == TrainingStatus.CANCELLED)
    //         .CountDocumentsAsync();
    //     return Convert.ToInt32(numOfElements);
    // }
    //
    // public async Task<IEnumerable<string>> GetTrainerClientIds(string trainerId)
    // {
    //     var trainings = await _context.Trainings.FindAsync(t => t.TrainerId == trainerId);
    //     return trainings.ToList().Select(t => t.ClientId).AsEnumerable();
    // }
    //
    // public async Task<IEnumerable<ClientTrainingDTO>> GetClientTrainings(string clientId)
    // {
    //     var clientTrainings = await _context.Trainings.FindAsync(t => t.ClientId == clientId);
    //     return _mapper.Map<IEnumerable<ClientTrainingDTO>>(clientTrainings.ToList());
    // }
}