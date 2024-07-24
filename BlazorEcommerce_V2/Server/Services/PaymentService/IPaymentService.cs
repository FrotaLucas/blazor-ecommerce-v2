using Stripe.Checkout;

namespace BlazorEcommerce_V2.Server.Services.PaymentService
{
    public interface IPaymentService
    {
        Task<Session> CreateCheckoutSession();
    }
}
