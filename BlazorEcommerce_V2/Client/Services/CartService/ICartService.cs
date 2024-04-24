namespace BlazorEcommerce_V2.Client.Services.CartService
{
    public interface ICartService 
    {
        //pq nao tem a palavra public em nenhuma das propriedades???????????? ?
        event Action OnChange;
        Task AddToCart(CartItem cartItem);
        Task<List<CartItem>> GetCartItems();   
        Task<List<CartProductResponse>> GetCartProducts();

        Task RemoveProductFromCart(int productId, int productTypeId);
    }
}
