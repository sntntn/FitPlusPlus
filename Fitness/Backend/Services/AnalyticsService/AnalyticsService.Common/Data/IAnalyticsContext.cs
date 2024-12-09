using AnalyticsService.Common.Entities;
using MongoDB.Driver;

namespace AnalyticsService.Common.Data;

public interface IAnalyticsContext
{
    IMongoCollection<Training> Trainings { get; set; }
}