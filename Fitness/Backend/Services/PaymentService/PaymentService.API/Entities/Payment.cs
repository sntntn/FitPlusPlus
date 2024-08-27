using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PaymentService.API.Entities
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public PaymentStatus Status { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TrainerPayPalEmail { get; set; }

    }
}
