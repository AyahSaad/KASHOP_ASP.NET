using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IManageUserService
    {
        Task<List<UserListResponse>> GetUsersAsync();
        Task<UserDetailsResponse> GetUserDetailsAsync(string userId);
        Task<BaseResponse> BlockedUserAsync (string userId);
        Task<BaseResponse> UnBlockedUserAsync(string userId);
        Task<BaseResponse> ChangeUserRoleAsync(ChangeUserRoleRequest request);

    }
}
