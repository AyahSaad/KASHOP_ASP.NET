using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IReviewService
    {
        Task <BaseResponse> AddReviewAsync (string userId, int productId,CreateReviewRequest request);
    }
}
