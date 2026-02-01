using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context=context;
        }
        public async Task<Order> CreateAsync(Order Request)
        {
            await _context.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;
        }

        public async Task<Order> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.SessionId == sessionId);
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(o => o.Product)
            .FirstOrDefaultAsync(o => o.Id  == orderId);
        }

        public async Task<List<Order>> GetOrdersByStatusAsync(OrderStatusEnum status)
        {
            return await _context.Orders
                .Where(o => o.OrderStatus == status)
                .Include(o => o.User)
                .ToListAsync();
        }

        public async Task<bool> HasUserDeliveredOrderForProduct(string userId, int productId)
        {
            return await _context.Orders
                .Where(o => o.UserId == userId &&
                            o.OrderStatus == OrderStatusEnum.Delivered)
                .SelectMany(o => o.OrderItems)
                .AnyAsync(oi => oi.ProductId == productId);

        }



    }
}
