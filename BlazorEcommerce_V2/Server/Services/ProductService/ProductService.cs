namespace BlazorEcommerce_V2.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
            private readonly DataContext _context;
            public ProductService(DataContext context)
            {
                _context = context;
            }

            public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
            {
                var response = new ServiceResponse<Product>();
                var product = await _context.Products.FindAsync(productId);

                if (product == null)
                {
                    response.Success = false;
                    response.Message = "Sorry, but this product does not exist.";
                }

                else
                {
                    response.Success = true;
                    response.Data = product;
                }

                return response;
            }
            public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
            {
                var response = new ServiceResponse<List<Product>>
                {
                    Data = await _context.Products.ToListAsync()
                };

                return response;
            }

            public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string category)
            {
                var response = new ServiceResponse<List<Product>>();

                var products = await _context.Products.Where(p => p.Category.Url.ToLower().Equals(category.ToLower())).ToListAsync();

                Console.WriteLine($"List of products: {products.Count}");
                if (products == null || products.Count == 0)
                {
                    response.Success = false;
                    response.Message = "List of products not found";
                    return response;
                }
                response.Data = products;
                return response;
            }


    }
    
}
