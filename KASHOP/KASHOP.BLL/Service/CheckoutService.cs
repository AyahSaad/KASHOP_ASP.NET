using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Repository;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CheckoutService : ICheckoutService
    {
        private readonly ICartRepository _cartRepository;

        public CheckoutService(ICartRepository cartRepository)
        {
            _cartRepository=cartRepository;
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

            if (request.PaymentMethod == "cash")
            {

                return new CheckoutResponse
                {
                    Success = true,
                    Message = "cash",
                };
            }

            else if (request.PaymentMethod == "visa")
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
        }
    }

