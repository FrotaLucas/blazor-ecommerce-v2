using BlazorEcommerce_V2.Server.Services.AuthService;
using BlazorEcommerce_V2.Server.Services.CartService;
using BlazorEcommerce_V2.Server.Services.OrderService;
using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce_V2.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService)
        {
            StripeConfiguration.ApiKey = "sk_test_51PfkfMHzpG9NkVZrBe9m6UDc44hNR0yjjF9IvHxwtVXPIItPHccny73NMcDrzEWVjgM9bb7Uimei8O6nwP74vXqq00gTnlyt6N";
            _authService = authService;
            _cartService = cartService;
            _orderService = orderService;
        }

        public async Task<Session> CreateCheckoutSession()
        {
            var products = (await _cartService.GetDbCartProducts()).Data;
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        Images = new List<string> { product.ImageUrl }
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                CustomerEmail = _authService.GetUserEmail(),
                PaymentMethodTypes = new List<string>
                {
                    "card"
                },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7003/order-success",
                CancelUrl = "https://localhost:7003/cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }
    }
}
