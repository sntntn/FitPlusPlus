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

    public async Task<IndividualTraining?> GetIndividualTrainingByReservationId(string reservationId)
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

    public async Task<GroupTraining?> GetGroupTrainingByReservationId(string reservationId)
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
}