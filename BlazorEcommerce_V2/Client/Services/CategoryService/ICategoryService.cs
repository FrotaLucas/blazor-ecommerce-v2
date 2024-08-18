﻿namespace BlazorEcommerce_V2.Client.Services.CategoryService
{
    public interface ICategoryService
    {
        event Action OnChange;
        List<Category> Categories { get; set; }
        List<Category> AdminCategories { get; set; }
        Task GetCategories();
        Task GetAdminCategories();

        Task AddCategory( Category category);
        Task UpdateCategory( Category category );   
        Task DeleteCategory (int categoryId);
        Category CreateNewCategory();

        //pq esta errado essa notacao ????????
        //Task<Category> GetByIdAsync(int id);

    }
}
