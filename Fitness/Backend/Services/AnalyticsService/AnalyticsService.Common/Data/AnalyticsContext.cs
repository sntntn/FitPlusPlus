using AnalyticsService.Common.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AnalyticsService.Common.Data;

public class AnalyticsContext : IAnalyticsContext
{
    public AnalyticsContext(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        var database = mongoClient.GetDatabase("AnalyticsDB");
        IndividualTrainings = database.GetCollection<IndividualTraining>("IndividualTrainings");
        GroupTrainings = database.GetCollection<GroupTraining>("GroupTrainings");
    }
    
    public IMongoCollection<IndividualTraining> IndividualTrainings { get; set; }
    public IMongoCollection<GroupTraining> GroupTrainings { get; set; }
}