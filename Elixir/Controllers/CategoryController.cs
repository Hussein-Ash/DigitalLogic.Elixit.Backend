using Elixir.DATA.DTOs.CategoryDto;
using Elixir.Extensions.StoreAuthoeization;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class CategoryController : BaseController
    {
        private readonly ICategoryService _service;
        public CategoryController(ICategoryService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<CategoryDto>>> GetAll([FromQuery] CategoryFilter filter) => OkPaginated(await _service.GetAllCategories(filter),filter.PageNumber,filter.PageSize);


        [Authorize(Roles = "Admin,SuperAdmin")]
        [StoreAuthorize]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CategoryForm form) => Ok(await _service.CreateCategory(form));
        
        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPost("{id}")]
        public async Task<ActionResult<CategoryDto>> CreateSubCategory(Guid id, [FromBody] SubCategoryForm form) => Ok(await _service.CreateSubCategory(form, id));

        [Authorize]
        [HttpGet("{categoryId}")]
        public async Task<ActionResult<Respons<CategoryDto>>> GetById(Guid categoryId) => Ok(await _service.GetCategoryById(categoryId));

        [Authorize]
        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> Delete(Guid categoryId) => Ok(await _service.DeleteCategory(categoryId));

        [Authorize(Roles = "Admin,SuperAdmin")]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(CategoryUpdate Update, Guid id) => Ok(await _service.UpdateCategory(Update, id));


    }
}
