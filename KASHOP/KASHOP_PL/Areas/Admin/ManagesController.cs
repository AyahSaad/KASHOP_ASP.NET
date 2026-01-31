using KASHOP.BLL.Service;
using KASHOP.DAL.DTO.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KASHOP.PL.Areas.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ManagesController : ControllerBase
    {
        private readonly IManageUserService _manageUserService;

        public ManagesController(IManageUserService manageUserService)
        {
            _manageUserService=manageUserService;
        }

        [HttpGet("users")]
        public async Task <IActionResult> GetUsers()
        {
            var result = await _manageUserService.GetUsersAsync();
            return Ok(result);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserDetails([FromRoute] string id)
        {
            var result = await _manageUserService.GetUserDetailsAsync(id);
            if (result == null) return NotFound("User not found");
            return Ok(result);
        }

        [HttpPatch("block/{id}")]
        public async Task<IActionResult> BlockUser([FromRoute]string id)=>
            Ok(await _manageUserService.BlockedUserAsync(id));

        [HttpPatch("unblock/{id}")]
        public async Task<IActionResult> UnBlockUser([FromRoute] string id) =>
            Ok(await _manageUserService.UnBlockedUserAsync(id));


        [HttpPatch("change-role")]
        [Authorize(Roles = "superAdmin")]

        public async Task<IActionResult> ChangeRole(ChangeUserRoleRequest request) =>
            Ok(await _manageUserService.ChangeUserRoleAsync(request));
    }
}
