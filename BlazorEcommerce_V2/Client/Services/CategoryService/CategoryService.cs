namespace BlazorEcommerce_V2.Client.Services.CategoryService
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _http;

        public CategoryService(HttpClient http)
        {
            _http = http;
        }
        public List<Category> Categories { get; set; }

        //Pq essa funcao nao precisa retornar tipo serviceResponse<List<Category>? ??????
        public async Task GetCategories()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Category>>>("/api/category");
            if (response != null && response.Data != null)
            {
                Categories = response.Data;
            }
        }
    }
}
