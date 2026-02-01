using KASHOP.DAL.Data;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class ProductRepository: IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.Include(c => c.Translations).Include(c => c.User).ToListAsync();
        }

        public async Task<Product?> FindByIdAsync(int id)
        {
            return await _context.Products.Include(c => c.Translations)
                .Include(c => c.SubImages)
                .Include(c => c.Reviews)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Product> AddAsync(Product Request)
        {
            await _context.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;
        }

        public async Task<bool> DecreaseQuantitiesAsync(List<(int productId, int quantity)> items)
        {
            var productIds = items.Select(p => p.productId).ToList();
            var products = await _context.Products.Where(p =>productIds.Contains(p.Id)).ToListAsync();
            foreach (var product in products)
            {
                var item= items.FirstOrDefault(p => p.productId == product.Id);
                if(product.Quantity < item.quantity)
                {
                    return false;
                }
                product.Quantity -= item.quantity;
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public IQueryable<Product> Query()
        {
            return _context.Products.Include(p => p.Translations).AsQueryable();
        }
    }
}
