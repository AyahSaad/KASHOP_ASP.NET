using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public interface IProductService
    {
        Task<List<ProductResponse>> GetAllProductsForAdmin();
        Task<ProductResponse> CreateProduct(ProductRequest request);
        Task<PaginatedResponse<ProductUserResponse>> GetAllProductsForUser(string lang = "en", int page = 1, int limit = 3, string? search = null,
            int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null, string? sortBy = null, bool asc = true);
        Task<ProductUserDetailsResponse> GetAllProductsDetailsForUser(int id, string lang = "en");

    }
}
