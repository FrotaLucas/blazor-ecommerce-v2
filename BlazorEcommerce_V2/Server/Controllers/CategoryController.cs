using BlazorEcommerce_V2.Server.Services.CategoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce_V2.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Category>>>> Get()
        {
            //preciso do wait aqui novamente mesmo sabendo que dentro de _categoryService esta sendo usado await?????
            var category = await _categoryService.GetCategories();
            return Ok(category);
        }
    }

        
    
}
