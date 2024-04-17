using BlazorEcommerce_V2.Shared;

namespace BlazorEcommerce_V2.Client.Services.ProductService
{
    public interface IProductService
    {
      
        event Action ProductsChanged;
        List<Product> Products { get; set; }
        string Message {  get; set; }
        int CurrentPage { get; set; }
        string LastSearchText { get; set; }
        public int PageCount { get; set; }
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int productId);
        Task SearchProducts( string searchText, int page);
        Task<List<string>> GetProductSearchSuggestions(string searchText);
    }
}
