using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public CheckoutService(ICartRepository cartRepository , IOrderRepository orderRepository, UserManager<ApplicationUser> userManager,IEmailSender emailSender)
        {
            _cartRepository=cartRepository;
            _orderRepository=orderRepository;
            _userManager=userManager;
            _emailSender=emailSender;
        }
        public async Task<CheckoutResponse> ProcessPaymentAsync(CheckoutRequest request, string userId)
        {
            var cartItems = await _cartRepository.GetUserCartAsync(userId);
            if (!cartItems.Any())
            {
                return new CheckoutResponse
                {
                    Success = false,
                    Message =" cart is empty"
                };
            }

            decimal totalAmount = 0;

            foreach (var cartItem in cartItems)
            {
                if (cartItem.Product.Quantity < cartItem.Count)
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "Out of stock"
                    };
                }
                totalAmount += cartItem.Product.Price * cartItem.Count;
            }

            // object not requset
            Order order = new Order
            {
                UserId = userId,
                PaymentMethod = request.PaymentMethod,
                AmountPaid = totalAmount,
            };

            if (request.PaymentMethod == PaymentMethodEnum.Cash)
            {

                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash",
                };
            }

            else if (request.PaymentMethod == PaymentMethodEnum.Visa)
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>(),
                    Mode = "payment",
                    SuccessUrl = $"https://localhost:7292/api/checkout/success?session_id={{CHECKOUT_SESSION_ID}}",
                    CancelUrl = $"https://localhost:7292/checkout/cancel",
                    Metadata = new Dictionary<string, string>
                    {
                        { "UserId", userId },
                    }
                };
                foreach (var cartItem in cartItems)
                {
                    options.LineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "USD",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = cartItem.Product.Translations.FirstOrDefault(t => t.Language == "en").Name,
                            },
                            UnitAmount = (long)cartItem.Product.Price * 100,
                        },
                        Quantity = cartItem.Count,

                    });
                }
                
                var service = new SessionService();
                var session = service.Create(options);
                order.SessionId = session.Id;

                await _orderRepository.CreateAsync(order);

                return new CheckoutResponse
                    {
                        Success = true,
                        Message = "payment session created",
                        Url = session.Url
                    };
                }
            else
                {
                    return new CheckoutResponse
                    {
                        Success = false,
                        Message = "invalid payment method"
                    };
                }
            }
       
   
        public async Task<CheckoutResponse> HandleSuccessAsync(string sessionId)
        {
            var service = new SessionService();
            var session = service.Get(sessionId);
            var userId = session.Metadata["UserId"];
            var order = await _orderRepository.GetBySessionIdAsync(sessionId);
            order.PaymentId = session.PaymentIntentId;
            order.OrderStatus = OrderStatusEnum.Approved;
            await _orderRepository.UpdateAsync(order);
            var user = await _userManager.FindByIdAsync(userId);
            await _emailSender.SendEmailAsync(user.Email, "Payment successfully", "<h2> Thank you ... </h2>");
            return new CheckoutResponse
            {
                Success = true,
                Message = "Payment completed successfully"
            };

        }
      }
    }

