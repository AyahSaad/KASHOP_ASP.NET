using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context=context;
        }

        public async Task<Category> CreateAsync(Category Request)
        {
           await _context.AddAsync(Request);
           await _context.SaveChangesAsync();
           return Request;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.Include(c => c.Translations).Include(c => c.User).ToListAsync();
        }

        public async Task<Category?> FindByIdAsync(int id)
        {
            return await _context.Categories.Include(c => c.Translations).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Category?> UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task DeleteAsync(Category category)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        
    }
}
