using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ReviewService:IReviewService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IOrderRepository  orderRepository, IReviewRepository reviewRepository)
        {
            _orderRepository=orderRepository;
            _reviewRepository=reviewRepository;
        }

        public async Task<BaseResponse> AddReviewAsync(string userId, int productId,CreateReviewRequest request)
        {
            var hasDeliveredOrder = await _orderRepository.HasUserDeliveredOrderForProduct(userId, productId);
            if (!hasDeliveredOrder)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "you can only review product you have received"
                };
            }
            var alreadyReview = await _reviewRepository.HasUserReviewedProduct(userId, productId);
            if (alreadyReview)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "can't add review"
                };
            }
            var review = request.Adapt<Review>();
            review.UserId = userId;
            review.ProductId = productId;
            await _reviewRepository.CreateAsync(review);
            return new BaseResponse
            {
                Success = true,
                Message = "review added successfully"
            };
        }
    }
}
