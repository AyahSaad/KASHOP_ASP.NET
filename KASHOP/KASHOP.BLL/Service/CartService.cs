using Azure.Core;
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
    public class CartService: ICartService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public CartService(IProductRepository productRepository,ICartRepository cartRepository)
        {
            _productRepository=productRepository;
            _cartRepository=cartRepository;
        }

        public async Task<BaseResponse> AddToCartAsync(string userId, AddToCartRequest request)
        {
            var product = await _productRepository.FindByIdAsync(request.ProductId);
            if (product is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "product not found"
                };
            }

            if (product.Quantity < request.Count)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "out of stock"
                };
            }

            var cart = request.Adapt<Cart>();
            cart.UserId = userId;
            await _cartRepository.CreateAsync(cart);
            return new BaseResponse()
            {
                Success= true,
                Message ="Product Added to Cart Successfully"
            };
        }
    }
}
