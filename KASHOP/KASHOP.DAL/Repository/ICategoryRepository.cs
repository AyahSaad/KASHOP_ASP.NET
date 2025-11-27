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
        List<Category> GetAll();
        Category Create(Category Request);
    }
}
