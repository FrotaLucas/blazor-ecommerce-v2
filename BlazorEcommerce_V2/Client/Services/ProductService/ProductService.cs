using BlazorEcommerce_V2.Shared;
using System.Net.Http.Json;

namespace BlazorEcommerce_V2.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public List<Product> Products { get; set; } = new List<Product>();

        public event Action ProductsChanged;
        public async Task<ServiceResponse<Product>> GetProduct(int productId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<Product>>($"api/product/{productId}");
            //pq essa tem return e a de baixo nao tem ???
            return result;
        }
        public async Task GetProducts(string? categoryUrl = null)
        {
            var result = (categoryUrl == null) ? await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>("api/product") :
                await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>($"api/product/category/{categoryUrl}");
            if (result != null && result.Data != null)
                Products = result.Data;

            //chamada de evento quando metodo GetProducts for acionado
            ProductsChanged.Invoke();
        }
     
    }
}
