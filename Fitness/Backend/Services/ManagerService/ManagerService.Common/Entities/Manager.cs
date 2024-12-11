using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ManagerService.Common.Entities;

public class Manager
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)] 
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    /*Enum za vrstu mandzera koji ce da odgovora za odredjenog trenera*/
}