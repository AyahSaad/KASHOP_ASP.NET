using KASHOP.DAL.DTO.Response;
using KASHOP.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.MapsterConfigurations
{
    public static class MapsterConfig
    {
        public static void MapsterConfiRegister()
        {

            //TypeAdapterConfig<Category, CategoryResponse>.NewConfig().Map(dest => dest., source => source.Id).TwoWays();

            TypeAdapterConfig<Category, CategoryResponse>.NewConfig()
                .Map(dest => dest.CreatedBy, source => source.User.UserName).TwoWays();

            TypeAdapterConfig<Category, CategoryUserResponse>.NewConfig()
              .Map(dest => dest.Name, source => source.Translations
              .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
              .Select(t => t.Name).FirstOrDefault());

            // In the response: convert image name to link 
            TypeAdapterConfig<Product, ProductResponse>.NewConfig()
                .Map(dest => dest.MainImage, source => $"https://localhost:7292/Images/{source.MainImage}");


            TypeAdapterConfig<Product, ProductUserResponse>.NewConfig()
              .Map(dest => dest.MainImage, source => $"https://localhost:7292/Images/{source.MainImage}")
              .Map(dest => dest.Name, source => source.Translations
              .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
              .Select(t => t.Name).FirstOrDefault());

            TypeAdapterConfig<Product, ProductUserDetailsResponse>.NewConfig()
             .Map(dest => dest.MainImage, source => $"https://localhost:7292/Images/{source.MainImage}")
             .Map(dest => dest.Name, source => source.Translations
             .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
             .Select(t => t.Name).FirstOrDefault())
             .Map(dest => dest.Description, source => source.Translations
             .Where(t => t.Language == MapContext.Current.Parameters["lang"].ToString())
             .Select(t => t.Description).FirstOrDefault());


            TypeAdapterConfig<Order, OrderResponse>.NewConfig()
                .Map(dest => dest.UserName, source => source.User.UserName);

            TypeAdapterConfig<Review, ReviewResponse>.NewConfig()
                .Map(dest => dest.UserName, source => source.User.UserName);


        }
    }

}
