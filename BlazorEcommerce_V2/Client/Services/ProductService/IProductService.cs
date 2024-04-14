using BlazorEcommerce_V2.Shared;

namespace BlazorEcommerce_V2.Client.Services.ProductService
{
    public interface IProductService
    {
        //evento criado para ajudar a perceber mudanca quando categoria mudar. Depois do componente NavMenu inicializado
        //ele nao recarrega novamente
        event Action ProductsChanged;
        //pq essa propriedade aqui dentro da interface ????? Ele esta sendo preenchido
        //atraves do metodo GetProduct no arquivo ProductService. Mas como isso ?
        List<Product> Products { get; set; }

        //string Message {  get; set; }

        //pq essse Task nao tem List ou ServiceResponse com encapsulamento ??????????
        Task GetProducts(string? categoryUrl = null);
        Task<ServiceResponse<Product>> GetProduct(int productId);

        //Task SearchProducts( string searchText);
        //Task<List<string>> GetProductSearchSuggestions(string searchText);
    }
}
