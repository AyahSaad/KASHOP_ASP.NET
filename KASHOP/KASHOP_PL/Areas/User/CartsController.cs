using Azure.Core;
using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace KASHOP.PL.Areas.User
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]

    public class CartsController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly ICartService _cartService;

        public CartsController(IStringLocalizer<SharedResource> localizer, ICartService cartService)
        {
            _localizer=localizer;
            _cartService=cartService;
        }

        [HttpPost("")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.AddToCartAsync(userId, request);
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.GetUserCartsAsync(userId);
            return Ok(result);
        }

        [HttpPatch("{productId}")]
        public async Task<IActionResult> UpdateQuantity([FromRoute] int productId,[FromBody] UpdateQuantityRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.UpdatQuantityAsync(userId,productId, request.Count);
            if (!result.Success)  return BadRequest(result); 
            return Ok(result);
        }

        [HttpDelete("")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.ClearCartAsync(userId);
            return Ok(result);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItemFromCart([FromRoute] int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _cartService.RemoveFromCartAsync(userId,productId);
            return Ok(result);
        }
    }
}
