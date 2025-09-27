using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.API.Entities;

public class GroupReservation
{
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Name { get; set; }
    public string About { get; set; }
    public string TrainerId { get; set; }
    public int Capacity { get; set; }
    public List<string> ClientIds { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DateOnly Date { get; set; }
}