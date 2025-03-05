using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.API.Models;

public class ChatSession
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public string TrainerId { get; set; }
    public string ClientId { get; set; }
    public bool IsUnlocked { get; set; } =  false;
    public List<Message> Messages { get; set; } =  new List<Message>();
    
    public DateTime? ExpirationDate { get; set; }
}