using Microsoft.AspNetCore.Mvc;
using PaymentService.API.Entities;
using PaymentService.API.Repositories;
using PaymentService.API.Services;

namespace PaymentService.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly PayPalClient _payPalClient;

        public PaymentController(IPaymentRepository paymentRepository, PayPalClient payPalClient)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _payPalClient = payPalClient ?? throw new ArgumentNullException(nameof(payPalClient));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Payment>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            var payments = await _paymentRepository.GetPaymentsAsync();

            return Ok(payments);
        }

        [HttpGet("{id}", Name = "GetPayment")]
        [ProducesResponseType(typeof(Payment), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Payment), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Payment>> GetPayment(string id)
        {
            var payment = await _paymentRepository.GetPaymentAsync(id);
            if (payment == null)
            {
                return NotFound();
            }
            return Ok(payment);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Payment), StatusCodes.Status201Created)]
        public async Task<ActionResult<Payment>> CreatePayment([FromBody] Payment payment)
        { 
            payment.Status = PaymentStatus.Pending;
            payment.PaymentDate = DateTime.UtcNow;
            await _paymentRepository.CreatePaymentAsync(payment);

            var paymentLink = await _payPalClient.CreatePaymentAsync(payment.Amount, payment.Currency, payment.TrainerPayPalEmail, payment.Id);
            if (string.IsNullOrEmpty(paymentLink))
            {
                return BadRequest("Failed to create payment.");
            }

            var paymentUrl = Url.Link("GetPayment", new { id = payment.Id });

            return Created(paymentUrl, new { Payment = payment, PaymentLink = paymentLink });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CapturePayment([FromBody] CapturePaymentRequest request)
        {
            var payment = await _paymentRepository.GetPaymentAsync(request.PaymentId);
            if (payment == null)
            {
                return NotFound("Payment not found.");
            }

            var captureResult = await _payPalClient.CapturePaymentAsync(request.Token);
            if (captureResult)
            {
                payment.Status = PaymentStatus.Completed;
                payment.PaymentDate = DateTime.UtcNow;
                await _paymentRepository.UpdatePaymentAsync(payment);

                return Ok("Payment captured successfully.");
            }

            return BadRequest("Failed to capture payment.");
        }

        [HttpDelete("{id}", Name = "DeletePayment")]
        [ProducesResponseType(typeof(Payment), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeletePayment(string id)
        {
            return Ok(await _paymentRepository.DeletePaymentAsync(id));

        }
    }
}
