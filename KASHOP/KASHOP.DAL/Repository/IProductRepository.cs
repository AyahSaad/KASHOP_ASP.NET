using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product> AddAsync(Product Request);
        Task<Product?> FindByIdAsync(int id);
        Task<bool> DecreaseQuantitiesAsync(List<(int productId, int quantity)> items);
        IQueryable<Product> Query();
    }
}
