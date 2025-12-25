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
    [Authorize(Roles= "Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryService _categoryService;

        public CategoriesController(IStringLocalizer<SharedResource> localizer, ICategoryService categoryService)
        {
            _localizer=localizer;
            _categoryService=categoryService;
        }

        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _categoryService.CreateCategory(request);
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
