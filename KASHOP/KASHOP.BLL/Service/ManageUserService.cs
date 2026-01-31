using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ManageUserService : IManageUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ManageUserService(UserManager<ApplicationUser> userManager)
        {
            _userManager=userManager;
        }
        public async Task<List<UserListResponse>> GetUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = users.Adapt<List<UserListResponse>>();
            for (int i = 0; i< users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                result[i].Roles = roles.ToList();

                result[i].IsBlocked =
           users[i].LockoutEnabled &&
           users[i].LockoutEnd.HasValue &&
           users[i].LockoutEnd > DateTimeOffset.UtcNow;

            }
            return result;
        }
        public async Task<UserDetailsResponse> GetUserDetailsAsync(string userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            var result = user.Adapt<UserDetailsResponse>();

            result.IsBlocked =
    user.LockoutEnabled &&
    user.LockoutEnd.HasValue &&
    user.LockoutEnd > DateTimeOffset.UtcNow;

            var roles = await _userManager.GetRolesAsync(user);
            result.Roles = roles.ToList();

            return result;
        }
        public async Task<BaseResponse> BlockedUserAsync(string userId)
        {
            var user =await  _userManager.FindByIdAsync(userId);
            await _userManager.SetLockoutEnabledAsync(user, true);
            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            await _userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = "User Blocked"
            };
        }
        public async Task<BaseResponse> UnBlockedUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.SetLockoutEnabledAsync(user, false);
            await _userManager.SetLockoutEndDateAsync(user, null);
            await _userManager.UpdateAsync(user);
            return new BaseResponse
            {
                Success = true,
                Message = "User Unblocked"
            };
        }
        public async Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            var cuurentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, cuurentRoles);
            await _userManager.AddToRoleAsync(user, request.Role);
            return new BaseResponse
            {
                Success = true,
                Message ="Role Updated"
            };
        }
    }
}
