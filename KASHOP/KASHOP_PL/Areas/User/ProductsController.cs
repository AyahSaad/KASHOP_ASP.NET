using KASHOP.BLL.Service;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> index([FromQuery] string lang = "en", [FromQuery] int page = 1, [FromQuery] int limit = 3, [FromQuery] string? search = null,
          [FromQuery] int? categoryId = null, [FromQuery] decimal? minPrice = null, [FromQuery] decimal? maxPrice = null, [FromQuery] string? sortBy = null, [FromQuery] bool asc = true)
        {
            var response = await _productService.GetAllProductsForUser(lang,page,limit,search,categoryId,minPrice,maxPrice,sortBy,asc);
            return Ok(new { message = _localizer["Success"].Value, response });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> index([FromRoute] int id,[FromQuery] string lang = "en")
        {
            var response = await _productService.GetAllProductsDetailsForUser(id,lang);
            return Ok(new { message = _localizer["Success"].Value, response });
        }
    }
}
