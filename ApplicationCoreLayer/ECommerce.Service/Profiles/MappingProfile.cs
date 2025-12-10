using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.Domain.Entities.BasketModule;
using ECommerce.Domain.Entities.ProductModule;
using ECommerce.Shared.Dtos.Baskets;
using ECommerce.Shared.Dtos.Products;

namespace ECommerce.Service.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Mapping Products Module

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductBrand, opt => opt.MapFrom(src => src.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());

            CreateMap<ProductBrand, BrandDto>();

            CreateMap<ProductType, TypeDto>();

            #endregion

            #region Mapping Basket Module

            // BasketDto → CustomerBasket
            CreateMap<BasketDto, CustomerBasket>()
                .ForMember(dest => dest.BasketItems, opt => opt.MapFrom(src => src.Items)).ReverseMap();

            // BasketItemDto → BasketItem
            CreateMap<BasketItemDto, BasketItem>()
                .ReverseMap();

            #endregion
        }
    }
}
