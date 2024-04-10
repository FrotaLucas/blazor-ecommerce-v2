namespace BlazorEcommerce_V2.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        List<Category> Categories { get; set; }
        Task GetCategories();
        //pq esta errado essa notacao ????????
        //Task<Category> GetByIdAsync(int id);

    }
}
