using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public enum OrderStatusEnum
    {
        Pending=1,
        Cancelled=2,
        Approved=3,
        Shipped=4,
        Delivered=5
    }
    public enum PaymentMethodEnum
    {
        Cash=1, Visa=2
    }
    public class Order
    {
        public int Id { get; set; }
        public OrderStatusEnum OrderStatus { get; set; } = OrderStatusEnum.Pending;
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ShippedDate { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentId { get; set; } 
        public decimal? AmountPaid {  get; set; } 
        public PaymentMethodEnum PaymentMethod { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
