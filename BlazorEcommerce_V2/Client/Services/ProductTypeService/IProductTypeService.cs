namespace BlazorEcommerce_V2.Client.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        event Action OnChange;

        public List<ProductType> ProductTypes { get; set; }
        public Task GetProductTypes();
    }
}
