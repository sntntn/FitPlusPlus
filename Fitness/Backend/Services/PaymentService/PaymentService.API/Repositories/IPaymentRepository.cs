using PaymentService.API.Entities;

namespace PaymentService.API.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetPaymentAsync(string id);
        Task<IEnumerable<Payment>> GetPaymentsAsync();
        Task CreatePaymentAsync(Payment payment);
        Task<bool> UpdatePaymentAsync(Payment payment);
        Task<bool> DeletePaymentAsync(string id);

    }
}
