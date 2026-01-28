using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Http.Features.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Response
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum paymentStatus { get; set; }
        public decimal AmountPaid { get; set; }
        public string UserName { get; set; }
    }
}
