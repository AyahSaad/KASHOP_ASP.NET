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
       Task<List<CategoryResponse>> GetAllCategories(string lang = "en");
       Task<CategoryResponse> CreateCategory(CategoryRequest Request);
       Task<BaseResponse> DeleteCategoryAsync(int id);
       Task<BaseResponse> UpdateCategoryAsync(int id, CategoryRequest request);
       Task<BaseResponse> ToggleStatusAsync(int id);

    }
}
