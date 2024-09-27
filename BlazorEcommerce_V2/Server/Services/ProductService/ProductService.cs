namespace BlazorEcommerce_V2.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
            private readonly DataContext _context;
            private readonly IHttpContextAccessor _httpContextAccessor;

            public ProductService(DataContext context, IHttpContextAccessor httpContextAccessor)
            {
                _context = context;
            _httpContextAccessor = httpContextAccessor;
            }

        public async Task<ServiceResponse<List<Product>>> GetFeaturedProducts()
        {
            var serviceResponse = new ServiceResponse<List<Product>>
            {
                Data = await _context.Products
                .Where(p => p.Featured && p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v => v.Visible && !v.Deleted) ).ToListAsync()
            };

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> GetAdminProducts()
        {
            var response = new ServiceResponse<List<Product>>()
            {
                Data = await _context.Products.Where(p => !p.Deleted)
                    .Include(p => p.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                    .ToListAsync()
            };

            return response;
        }

        public async Task<ServiceResponse<Product>> GetProductAsync(int productId)
            {
                var response = new ServiceResponse<Product>();
                Product product = null;

            if (_httpContextAccessor.HttpContext.User.IsInRole("Admin")  )
            {
                product = await _context.Products.Include(p => p.Variants.Where(v => !v.Deleted))
                    .ThenInclude(v => v.ProductType)
                    .FirstOrDefaultAsync( p => p.Id == productId && !p.Deleted );
            }
            else
            {
                //op1
                     product = await _context.Products
                    .Include(p => p.Variants.Where(v=> v.Visible && !v.Deleted ))
                    .ThenInclude(pt => pt.ProductType)
                    .FirstOrDefaultAsync(p => p.Id == productId && p.Visible && !p.Deleted);
            }

            //op2
            //var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
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
                    Data = await _context.Products
                    .Where(p=> p.Visible && !p.Deleted)
                    .Include(p=> p.Variants.Where(v=> v.Visible && !v.Deleted)).ToListAsync()
                };

                return response;
            }

            public async Task<ServiceResponse<List<Product>>> GetProductsByCategory(string categoryUrl)
            {
                var response = new ServiceResponse<List<Product>>();

                var products = await _context.Products
                .Where(p => p.Category.Url.ToLower().Equals(categoryUrl.ToLower()) && p.Visible && !p.Deleted)
                .Include(p => p.Variants.Where(v=> v.Visible && !v.Deleted))
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

        public async Task<ServiceResponse<ProductSearchResult>> SearchProducts(string searchText, int page)
        {
            var maxProductsPerPage = 2f;
            var pageCounts = Math.Ceiling((await FindProductsBySeachText(searchText)).Count / maxProductsPerPage);
            var products = await _context.Products
                              .Where(p => p.Title.ToLower().Contains(searchText.ToLower()) ||
                                  p.Description.ToLower().Contains(searchText.ToLower()) 
                                  && p.Visible && p.Deleted) 
                              .Include(p => p.Variants)
                              .Skip((page - 1) * (int)maxProductsPerPage)
                              .Take((int)maxProductsPerPage)
                              .ToListAsync();

            ServiceResponse<ProductSearchResult> response = new ServiceResponse<ProductSearchResult>()
            {
                Data = new ProductSearchResult
                {
                    Products = products,
                    CurrentPage = page,
                    Pages = (int)pageCounts
                }
            };


            return response;
        }

        private async Task<List<Product>> FindProductsBySeachText(string searchText)
        {
            return await _context.Products.Where(p => p.Title.ToLower().Contains(searchText.ToLower())
                            ||
                            p.Description.ToLower().Contains(searchText.ToLower())
                            && p.Visible && !p.Deleted
                            )
                            .Include(p => p.Variants)
                            .ToListAsync();
        }
    }
    
}
