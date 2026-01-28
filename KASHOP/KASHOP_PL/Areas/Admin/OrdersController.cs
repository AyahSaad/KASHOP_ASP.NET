using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IOrderService _orderService;

        public OrdersController(IStringLocalizer<SharedResource> localizer, IOrderService orderService)
        {
            _localizer=localizer;
            _orderService=orderService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderStatusEnum status = OrderStatusEnum.Pending)
        {
            var orders = await _orderService.GetOrdersAsync(status);
            return Ok(orders);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> UpdateStatus(int orderId,[FromBody] UpdateOrderStatusRequest request)
        {
            var result = await _orderService.UpdatOrderStatusAsync(orderId,request.Status);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }
    }
}
