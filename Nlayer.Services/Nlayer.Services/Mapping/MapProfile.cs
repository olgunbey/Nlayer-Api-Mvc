using AutoMapper;
using Nlayer.Core.DTOs;
using Nlayer.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nlayer.Services.Mapping
{
    public class MapProfile:Profile
    {
        public MapProfile()
        {
            //productdto yu product çevir
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<ProductFeature, ProductFeatureDTO>().ReverseMap();
            CreateMap<Product,ProductCategoryDTO>();
            CreateMap<Category, CategoryWithProductDTO>();


        }
    }
}
