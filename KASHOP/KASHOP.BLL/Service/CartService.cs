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

            var cartItem = await _cartRepository.GetCartItemAsync(userId, request.ProductId);
            if (cartItem is not null)
            {
                cartItem.Count+= request.Count;
                await _cartRepository.UpdateAsync(cartItem);
            } else
            {
                var cart = request.Adapt<Cart>();
                cart.UserId = userId;
                await _cartRepository.CreateAsync(cart);
            }
            return new BaseResponse()
            {
                Success= true,
                Message ="Product Added to Cart Successfully"
            };
        }

        public async Task<CartSummaryResponse> GetUserCartsAsync(string userId, string lang = "en")
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            var items = cartItems.Select(c => new CartResponse
            {
                ProductId = c.ProductId,
                ProductName = c.Product.Translations.FirstOrDefault(t => t.Language == lang).Name,
                Count = c.Count,
                Price= c.Product.Price
            }).ToList();

            return new CartSummaryResponse
            {
                Items = items,
            };
        }
    }
}
