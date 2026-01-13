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
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context=context;
        }
        public async Task<Cart> CreateAsync(Cart Request)

        {
            await _context.AddAsync(Request);
            await _context.SaveChangesAsync();
            return Request;
        }

    }
}
