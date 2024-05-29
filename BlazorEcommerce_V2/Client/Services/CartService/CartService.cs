using BlazorEcommerce_V2.Shared;
using Blazored.LocalStorage;

namespace BlazorEcommerce_V2.Client.Services.CartService
{
    public class CartService : ICartService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public CartService(ILocalStorageService localStorage, HttpClient http, AuthenticationStateProvider authenticationStateProvider)
        {
            _localStorage = localStorage;   
            _http = http;
            _authenticationStateProvider = authenticationStateProvider;
        }
        public event Action OnChange;
        private async Task<bool> IsUserAuthenticated()
        {
            return (await _authenticationStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }

        public async Task AddToCart(CartItem cartItem)
        {
            if (await IsUserAuthenticated())
            {
                Console.WriteLine("User is authenticated");
            }
            else
            {
                Console.WriteLine("User is NOT authenticated");

            }


            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart == null)
            {
                cart = new List<CartItem>();
            }

            var sameItem = cart.Find(x => x.ProductId == cartItem.ProductId &&
            x.ProducTypetId == cartItem.ProducTypetId);

            if (sameItem == null)
            {
                cart.Add(cartItem);
            }
            else
            {
                //sameItem.Quantity += cartItem.Quantity;
                sameItem.Quantity = sameItem.Quantity + 1;
            }

            await _localStorage.SetItemAsync("cart", cart);
            await GetCartItemsCount();
        }


        public async Task<List<CartItem>> GetCartItems()
        {
            await GetCartItemsCount();
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart == null)
            {
                cart = new List<CartItem>();
            }
            return cart;
        }
        //busca no banco de dados infos como preco e Product Type para expor no componente Cart.razor 
        public async Task<List<CartProductResponse>> GetCartProducts()
        {
            var cartItems = await _localStorage.GetItemAsync<List<CartItem>>("cart");
            //estou uando esse metodo pq a api eh do tipo Post
            var response = await _http.PostAsJsonAsync("api/cart/products", cartItems);
            var cartProducts = await response.Content.ReadFromJsonAsync<ServiceResponse<List<CartProductResponse>>>();

            return cartProducts.Data;
        }

        public async Task RemoveProductFromCart(int productId, int productTypeId)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if(cart == null)
            {
                return;
            }

            var cartItem = cart.Find(x => x.ProductId == productId && x.ProducTypetId == productTypeId);
           
            if(cartItem != null)
            {
                cart.Remove(cartItem); 
                await _localStorage.SetItemAsync("cart", cart);
                await GetCartItemsCount();
            }
        }
        //salva no banco de dados a lista de produtos
        public async Task StoreCartItems(bool emptyLocalCart = false)
        {
            //nao deveria aqui ser cartItemsCount ao inves de cart ? Quando Login.razor eh chamado o usuario
            //esta autenticado e por conseguencia deveria salvar itens personalizados do carrinho no cartItemsCount 
            //???????????????????????
            var localCart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (localCart == null)
                return;

            await _http.PostAsJsonAsync("api/cart", localCart);

            if(emptyLocalCart)
            {
                await _localStorage.RemoveItemAsync("cart");
            }
        }

        //essa funcao tem return ?
        public async Task UpdateQuantity(CartProductResponse product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartItem>>("cart");

            if (cart == null)
            {
                return;
            }

            var cartItem = cart.Find(x => x.ProductId == product.ProductId
            && x.ProducTypetId == product.ProductTypeId);

            if (cartItem != null)
            {   //Interessante. Ao mudar item cartItem que pertence a lista.
                //automaticamente a lista cart eh atualizada
                cartItem.Quantity = product.Quantity;
                //pq precisa desse _localStorage aqui ?
                await _localStorage.SetItemAsync("cart", cart);
            }
        }

        //extrai do banco de dados os items de carrinho, conta quantos tem e salva no localSotorage em cartItemsCount.
        //Descobre o Id do User usando o HttpContext
        public async Task GetCartItemsCount()
        {
            //todos os metodos dessa classe precisam chamar o GetCartItemsCount pq ele tem o INVOKE que atualiza
            //tudo.

            if (await IsUserAuthenticated())
            {
                var cartItems = await _http.GetFromJsonAsync<ServiceResponse<int>>("api/cart/count");
                var count = cartItems.Data;
                await _localStorage.SetItemAsync<int>("cartItemsCount", count);
            }
            else
            {
                var cartCount = await _localStorage.GetItemAsync<List<CartItem>>("cart");
                await _localStorage.SetItemAsync<int>("cartItemsCount", cartCount != null ? cartCount.Count : 0);
            }
          
            OnChange.Invoke();
        }
    }
}
