using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllAsync();
        Task<Category> CreateAsync(Category Request);
        Task<Category?> FindByIdAsync(int id);
        Task DeleteAsync(Category category);
        Task<Category?> UpdateAsync(Category category);
    }
}
