using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace PaymentService.API.Services
{
    public class PayPalClient
    {
        private readonly PayPalEnvironment _environment;
        private readonly PayPalHttpClient _client;

        public PayPalClient(IConfiguration configuration)
        {
            _environment = new SandboxEnvironment(
                configuration["PayPalSettings:ClientId"],
                configuration["PayPalSettings:ClientSecret"]
            );
            _client = new PayPalHttpClient(_environment);
        }

        public async Task<string> CreatePaymentAsync(decimal amount, string currency, string trainerPayPalEmail, string paymentId)
        {
            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(new OrderRequest
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>
                {
                    new PurchaseUnitRequest
                    {
                        AmountWithBreakdown = new AmountWithBreakdown
                        {
                            CurrencyCode = currency,
                            Value = amount.ToString("F")
                        },
                        Payee = new Payee
                        {
                            Email = trainerPayPalEmail
                        }
                    }
                },
                ApplicationContext = new ApplicationContext
                {
                    ReturnUrl = $"http://localhost:8080/payment-success?paymentId={paymentId}",
                    CancelUrl = "http://localhost:8080/payment-cancel"
                }
            });

            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            return result.Links.FirstOrDefault(link => link.Rel == "approve")?.Href;
        }

        public async Task<bool> CapturePaymentAsync(string orderId)
        {
            var request = new OrdersCaptureRequest(orderId);
            request.RequestBody(new OrderActionRequest());
            
            var response = await _client.Execute(request);
            var result = response.Result<Order>();

            return result.Status == "COMPLETED";
        }

    }
}
