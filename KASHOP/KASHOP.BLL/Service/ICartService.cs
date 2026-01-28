using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICartService
    {
        Task<BaseResponse> AddToCartAsync(string userId,AddToCartRequest request);
        Task<CartSummaryResponse> GetUserCartsAsync(string userId,string lang = "en");
        Task<BaseResponse> ClearCartAsync(string userId);
        Task<BaseResponse> RemoveFromCartAsync(string userId, int productId);
        Task<BaseResponse> UpdatQuantityAsync(string userId, int productId, int count);
    }
}
