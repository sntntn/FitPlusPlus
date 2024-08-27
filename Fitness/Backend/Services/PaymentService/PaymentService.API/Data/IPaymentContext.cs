using MongoDB.Driver;
using PaymentService.API.Entities;

namespace PaymentService.API.Data
{
    public interface IPaymentContext
    {
        IMongoCollection<Payment> Payments { get; }
    }
}
