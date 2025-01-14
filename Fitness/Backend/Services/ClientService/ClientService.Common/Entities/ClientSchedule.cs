using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.Extensions.Configuration;

namespace ClientService.Common.Entities
{
    public class ClientSchedule
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("ClientId")]
        public string ClientId { get; set; }
        public List<WeeklySchedule> WeeklySchedules { get; set; } = new List<WeeklySchedule>();
        public ClientSchedule(string id)
        {
            var startWeek = 1;
            var endWeek = 3;
            ClientId = id;
            for (int i = startWeek; i <= endWeek; i++)
            {
                WeeklySchedules.Add(new WeeklySchedule(i));
            }
        }
    }
}
