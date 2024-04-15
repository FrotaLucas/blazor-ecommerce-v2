using BlazorEcommerce_V2.Server.Services.ProductService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce_V2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProducts()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }
        [HttpGet("{productId}")]
        public async Task<ActionResult<ServiceResponse<Product>>> GetProduct(int productId)
        {
            var product = await _productService.GetProductAsync(productId);

            return Ok(product);
        }

        [HttpGet("category/{categoryUrl}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductByCategory(string categoryUrl)
        {
            var response = await _productService.GetProductsByCategory(categoryUrl);

            return Ok(response);
        }
        
        [HttpGet("search/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> SearchProduct(string searchText)
        {
            var response = await _productService.SearchProducts(searchText);

            return Ok(response);
        }

        [HttpGet("searchsuggentions/{searchText}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductSearchSuggestions(string searchText)
        {
            //ATENCAO. O RETORNO Do BD nao eh <List<Product> mas sim <List<striing>. Nao eh um erro????
            var response = await _productService.GetProductSearchSuggestions(searchText);

            return Ok(response);
        }

    }
}
