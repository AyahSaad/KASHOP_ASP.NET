using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface ICategoryService
    {
       List<CategoryResponse> GetAllCategories();
       CategoryResponse CreateCategory(CategoryRequest Request);
       Task<BaseResponse> DeleteCategoryAsync(int id);
       Task<BaseResponse> UpdateCategoryAsync(int id, CategoryRequest request);

    }
}
