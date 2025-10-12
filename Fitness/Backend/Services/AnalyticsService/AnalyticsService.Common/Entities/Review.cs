using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnalyticsService.Common.Entities;

public class Review
{
    public string? UserId { get; set; }
    public int? Rating { get; set; }
    public string? Comment { get; set; }
}