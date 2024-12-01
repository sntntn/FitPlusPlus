using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.API.Models
{
    public class Message
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)] // prikazuje `Id` kao string u JSON-u, ali ga MongoDB cuva kao ObjectId
        public string Id { get; set; }
        public string Content { get; set; }

        //[BsonElement("timestamp")] // ako hocu da mi se drugacije zove u bazi 
        public DateTime Timestamp { get; set; }
    }
}