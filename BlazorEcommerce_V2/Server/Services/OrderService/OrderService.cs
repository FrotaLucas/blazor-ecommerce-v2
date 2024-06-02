using BlazorEcommerce_V2.Server.Services.CartService;
using System.Security.Claims;

namespace BlazorEcommerce_V2.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        public readonly DataContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public readonly ICartService _cartService;
        public OrderService(DataContext context, IHttpContextAccessor httpContextAccessor, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<bool>> PlaceOrder()
        {
              
            
            Console.WriteLine("UserId" + GetUserId()); 

            var products = (await _cartService.GetDbCartProducts()).Data;
            decimal totalPrice = 0;
            products.ForEach(product => totalPrice += product.Price * product.Quantity);

            var orderItems = new List<OrderItem>();

            //criando uma lista de OrderItems
            products.ForEach(product => orderItems.Add(new OrderItem
            {
                ProductId = product.ProductId,
                ProductTypeId = product.ProductTypeId,
                Quantity = product.Quantity,
                TotalPrice = product.Price * product.Quantity,

            }));

            //criando uma Order com UsedId e fazendo a conexao com OrderItems. 
            //Como a conexao estabelecida, ao salvar na tabela Orders, a tabela OrderItems automaticamente
            //tambem vai ser preenchida!!
            var order = new Order()
            {
                UserId = GetUserId(),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
                OrderItems = orderItems
            };

            await _context.Orders.AddAsync(order);

            _context.CartItems.RemoveRange(_context.CartItems
                .Where( ci => ci.UserId == GetUserId()));

            await _context.SaveChangesAsync();

            return new ServiceResponse<bool>
            {
                Data = true
            };
        }
    }
}
