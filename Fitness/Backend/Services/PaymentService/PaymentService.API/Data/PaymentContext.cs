using MongoDB.Driver;
using PaymentService.API.Entities;

namespace PaymentService.API.Data
{
    public class PaymentContext : IPaymentContext
    {
        public PaymentContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase("PaymentDB");

            Payments = database.GetCollection<Payment>("Payments");

        }

        public IMongoCollection<Payment> Payments { get; }
    }
}
