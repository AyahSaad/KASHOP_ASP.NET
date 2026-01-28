using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface ICartRepository
    {
        Task<Cart> CreateAsync(Cart Request);
        Task<List<Cart>> GetUserCartAsync(string userId);
        Task<Cart?> GetCartItemAsync(string userId, int productId);
        Task<Cart> UpdateAsync(Cart cart);
        Task ClearCartAsync(string userId);
        Task RemoveFromCartAsync(Cart cart);
    }
}
