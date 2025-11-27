using Azure;
using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using KASHOP.DAL.Repository;


namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(IStringLocalizer<SharedResource>localizer,ICategoryRepository categoryRepository)
        {
            _localizer=localizer;
            _categoryRepository=categoryRepository;
        }

        [HttpGet("")]
        public IActionResult index()
        {
            return Ok(new { message = _localizer["Success"].Value, response });
        }


        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
           _categoryRepository.Create(category);
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
