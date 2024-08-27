namespace PaymentService.API.Entities
{
    public class CapturePaymentRequest
    {
        public string PaymentId { get; set; }
        public string Token { get; set; }
    }
}
