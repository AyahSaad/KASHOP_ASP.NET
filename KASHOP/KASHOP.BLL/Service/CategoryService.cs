using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository=categoryRepository;
        }
        public CategoryResponse CreateCategory(CategoryRequest Request)
        {

            var category = Request.Adapt<Category>();
            _categoryRepository.Create(category);
            return category.Adapt<CategoryResponse>();
        }

        public List<CategoryResponse> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();
            return response;
        }

        public async Task<BaseResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _categoryRepository.FindByIdAsync(id);
                if (category is null)
                {
                    return new BaseResponse
                    {
                        Success = false,
                        Message = "Category not found"
                    };
                }

                await _categoryRepository.DeleteAsync(category);
                return new BaseResponse
                {
                    Success = true,
                    Message = "Category Deleted Successfully"
                };

            }
            catch (Exception ex) {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Can't Delete Category",
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }
}
