using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ReservationService.API.Entities;

public class IndividualReservation
{
    [BsonId] 
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string ClientId { get; set; }
    public string TrainerId { get; set; }
    public string TrainingTypeId { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public DateOnly Date { get; set; }
    public IndividualReservationStatus Status { get; set; } = IndividualReservationStatus.Active;
}