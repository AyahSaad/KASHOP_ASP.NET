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


namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(ApplicationDbContext context, IStringLocalizer<SharedResource>localizer)
        {
            _context=context;
            _localizer=localizer;
        }

        [HttpGet("")]
        public IActionResult index()
        {
            var categories = _context.Categories.Include(c=>c.Translations).ToList();
            var response = categories.Adapt<List<CategoryResponse>>();
            return Ok(new { message = _localizer["Success"].Value, response });
        }


        [HttpPost("")]
        public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _context.Add(category);
            _context.SaveChanges();
            return Ok(new { message = _localizer["Success"].Value });
        }
    }
}
