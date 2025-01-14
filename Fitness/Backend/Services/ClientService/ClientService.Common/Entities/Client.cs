using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ClientService.Common.Entities
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
       
    }
}
