using Elixir.DATA.DTOs.ProductCategory;
using Elixir.Services;
using Elixir.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elixir.Controllers
{

    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _service;
        public ProductCategoryController(IProductCategoryService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Respons<ProductCategoryDto>>> GetAll([FromQuery] ProductCategoryFilter filter) => Ok(await _service.GetAll(filter));


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductCategoryDto>> Create([FromBody] ProductCategoryForm form) => Ok(await _service.Add(form));

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Respons<ProductCategoryDto>>> GetById(Guid id) => Ok(await _service.GetById(id));

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id) => Ok(await _service.Delete(id));

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(ProductCategoryUpdate update, Guid id) => Ok(await _service.Update(id, update));


    }
}
