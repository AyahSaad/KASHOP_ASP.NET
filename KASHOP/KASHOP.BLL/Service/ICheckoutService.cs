using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICheckoutService
    {
        Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId);
        Task<CheckoutResponse> HandleSuccessAsync(string sessionId);
    }
}
