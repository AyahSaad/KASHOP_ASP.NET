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
            var cartItem = await _cartRepository.GetCartItemAsync(userId, request.ProductId);
            var existingCount = cartItem?.Count ?? 0;


            if (product is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "product not found"
                };
            }
            
            if (product.Quantity < (existingCount + request.Count))
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message = "out of stock"
                };
            }

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

        public async Task<BaseResponse> ClearCartAsync(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            return new BaseResponse()
            {
                Success = true,
                Message ="cart cleared successfully"
            };
        }

        public async Task<BaseResponse> RemoveFromCartAsync(string userId, int productId)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId, productId);
            if (cartItem is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message ="cart item not found"
                };
            }
            await _cartRepository.RemoveFromCartAsync(cartItem);
            return new BaseResponse()
            {
                Success = true,
                Message ="cart item removed successfully"
            };
        }

        public async Task<BaseResponse> UpdatQuantityAsync(string userId, int productId,int count)
        {
            var cartItem = await _cartRepository.GetCartItemAsync(userId,productId);
            var product = await _productRepository.FindByIdAsync(productId);
            if (cartItem is null)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message ="cart item is not found"
                };
            }

            if (count < 0)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message ="invalid count"
                };
            }
            if (count == 0)
            {
                await _cartRepository.RemoveFromCartAsync(cartItem);
                return new BaseResponse()
                {
                    Success = true,
                    Message ="item removed successfully"
                };
            }
            if (product.Quantity < count)
            {
                return new BaseResponse()
                {
                    Success = false,
                    Message ="out of stock"
                };
            }
            cartItem.Count = count;
            await _cartRepository.UpdateAsync(cartItem);
            return new BaseResponse()
            {
                Success = true,
                Message ="cart item quantity updated successfully"
            };
        }
    }
}
