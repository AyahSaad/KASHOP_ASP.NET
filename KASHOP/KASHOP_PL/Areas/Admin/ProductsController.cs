using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IProductService _productService;

        public ProductsController(IStringLocalizer<SharedResource> localizer, IProductService productService)
        {
            _localizer=localizer;
            _productService=productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> index()
        {
            var response = await _productService.GetAllProductsForAdmin();
            return Ok(new { message = _localizer["Success"].Value, response });
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            var response = await _productService.CreateProduct(request);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
