using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order Request);
        Task<Order> GetBySessionIdAsync(string SessionId);
        Task<Order> UpdateAsync(Order order);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status);
        Task<bool> HasUserDeliveredOrderForProduct(string userId, int productId);
    }
}
