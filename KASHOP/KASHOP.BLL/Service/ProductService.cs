using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IFileService _fileService;

        public ProductService(IProductRepository productRepository, IFileService fileService)
        {
            _productRepository=productRepository;
            _fileService=fileService;
        }

        //Service ⇄ DTO
        // Repository ⇄ Entity

        /* 
            Controller
            ↓ inject
            ProductService (IProductService)
            ↓ inject
            ProductRepository (IProductRepository)
            ↓ inject
            ApplicationDbContext
            FileService (IFileService)
         */

        public async Task<List<ProductUserResponse>> GetAllProductsForUser(string lang = "en", int page=1,int limit=3, string? search = null)
        {
            var query = _productRepository.Query();
            if(search is not null)
            {
                query= query.Where(p => p.Translations.Any(t => t.Language == lang && t.Name.Contains(search) || t.Description.Contains(search)) );
            }
            var totalCount = await query.CountAsync();
            query= query.Skip((page-1) * limit).Take(limit);
            var response = query.BuildAdapter().AddParameters("lang", lang).AdaptToType<List<ProductUserResponse>>();
            return response;
        }

        public async Task<ProductUserDetailsResponse> GetAllProductsDetailsForUser(int id,string lang = "en")
        {
            var product = await _productRepository.FindByIdAsync(id);
            var response = product.BuildAdapter().AddParameters("lang", lang).AdaptToType<ProductUserDetailsResponse>();
            return response;
        }
        public async Task<List<ProductResponse>> GetAllProductsForAdmin()
        {
            var products = await _productRepository.GetAllAsync();
            var response = products.Adapt<List<ProductResponse>>();
            return response;
        }
        public async Task<ProductResponse> CreateProduct(ProductRequest request)
        {
            // Service (CreateProduct) Map DTO → Entity
            var product = request.Adapt<Product>();
            // Optional File Upload
            if (request.MainImage != null)
            {
                var imagePath = await _fileService.UploadAsync(request.MainImage);
                product.MainImage = imagePath; 
            }

            //Sub Images 
            if (request.SubImages != null)
            {
                product.SubImages = new List<ProductImage>();
                foreach (var file in request.SubImages) { 
                    var imagePath = await _fileService.UploadAsync(file);
                    product.SubImages.Add(new ProductImage
                    {
                        ImageName = imagePath
                    });
                }
            }
            // Repository.AddAsync(Entity)
            await _productRepository.AddAsync(product);
            // Map Entity → Response DTO
            return product.Adapt<ProductResponse>();
        }
    }
}
