using AnalyticsService.Common.Entities;
using MongoDB.Driver;

namespace AnalyticsService.Common.Data;

public interface IAnalyticsContext
{
    IMongoCollection<IndividualTraining> IndividualTrainings { get; set; }
    IMongoCollection<GroupTraining> GroupTrainings { get; set; }
}