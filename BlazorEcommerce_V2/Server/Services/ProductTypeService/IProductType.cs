using System.Security.Permissions;

namespace BlazorEcommerce_V2.Server.Services.ProductTypeService
{
    public interface IProductTypeService
    {
        Task<ServiceResponse<List<ProductType>>> GetProductTypes();

    }
}
