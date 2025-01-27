using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.API.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public string Content { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string SenderType { get; set; } = string.Empty;
    }
}