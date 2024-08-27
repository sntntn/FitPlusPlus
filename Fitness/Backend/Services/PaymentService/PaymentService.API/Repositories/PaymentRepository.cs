using MongoDB.Driver;
using PaymentService.API.Data;
using PaymentService.API.Entities;

namespace PaymentService.API.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly IPaymentContext _context;

        public PaymentRepository(IPaymentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Payment> GetPaymentAsync(string id)
        {
            return await _context.Payments.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsAsync()
        {
            return await _context.Payments.Find(p => true).ToListAsync();
        }

        public async Task CreatePaymentAsync(Payment payment)
        {
            await _context.Payments.InsertOneAsync(payment);
        }

        public async Task<bool> UpdatePaymentAsync(Payment payment)
        {
            var updateResult = await _context.Payments.ReplaceOneAsync(p => p.Id == payment.Id, payment);
            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;

        }

        public async Task<bool> DeletePaymentAsync(string id)
        {
            var deleteResult = await _context.Payments.DeleteOneAsync(p => p.Id == id);
            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
