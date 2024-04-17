namespace BlazorEcommerce_V2.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
            private readonly DataContext _context;
            public ProductService(DataContext context)
            {
                _context = context;
            }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            //ServiceResponse<List<Product>> serviceResponse = new ServiceResponse<List<Product>>();
            //var result = await _context.Products.Where(p => p.Featured).Include(p => p.Variants).ToListAsync();

            //if (result != null)
            //{
            //    serviceResponse.Data = result;
            //}

            var serviceResponse = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products.Where(p => p.Featured).Include(p => p.Variants).ToListAsync()
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
            {
                var response = new ServiceResponse<Product>();
                var product = await _context.Products.Include(p=> p.Variants).
                ThenInclude(v => v.ProductType).FirstOrDefaultAsync(p=> p.Id == productId);
                //var product = await _context.Products.FindAsync(productId);

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
                    Data = await _context.Products.Include(p=> p.Variants).ToListAsync()
                };

                return response;
            }

            public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
            {
                var response = new ServiceResponse<List<Product>>();

                var products = await _context.Products.Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()))
                .Include(p => p.Variants)
                .ToListAsync();

                if (products == null || products.Count == 0)
                {
                    response.Success = false;
                    response.Message = "List of products not found";
                    return response;
                }
                response.Data = products;
                return response;
            }

        public async Task<ServiceResponse<List<string>>> GetProductSearchSuggestions(string searchText)
        {
            var products = await FindProductsBySeachText(searchText);
            List<string> result = new List<string>();

            foreach (var product in products)
            {
                if(product.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)) 
                {
                    result.Add(product.Title);
                }

                if (product.Description != null)
                {
                    var pontuaction = product.Description.Where(char.IsPunctuation)
                    .Distinct().ToArray();

                    var words = product.Description.Split()
                    .Select(s => s.Trim(pontuaction));

                    foreach (var word in words)
                    {
                        if(word.Contains(searchText, StringComparison.OrdinalIgnoreCase) && !result.Contains(word))
                        {
                            result.Add(word);
                        }
                    }
                }
            }


             return new ServiceResponse<List<string>> { Data = result };
        }

        public async Task<ServiceResponse<List<Product>>> SearchProducts(string searchText)
        {
            ServiceResponse<List<Product>> response = new ServiceResponse<List<Product>>()
            {
                Data = await FindProductsBySeachText(searchText)
            };

            return response;
        }

        private async Task<List<Product>> FindProductsBySeachText(string searchText)
        {
            return await _context.Products.Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                            ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                            )
                            .Include(p => p.Variants)
                            .ToListAsync();
        }
    }
    
}
