using AutoMapper;
using ProductCatalogAPI.Application.DTO.Product;
using ProductCatalogAPI.Application.DTO.ProductImage;
using ProductCatalogAPI.Domain.Entities;

namespace ProductCatalogAPI.Application.Mapper
{
    public class GlobalMapperProfile: Profile
    {
        public GlobalMapperProfile()
        {
            CreateMap<ProductCreateDto, Product>();

            CreateMap<ProductUpdateDto, Product>();

            CreateMap<Product, ProductListDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImage.Select(x => new ProductImageListDto
                {
                    Id = x.Id,
                    FileName = x.FileName,
                    ImageURL = x.ImageURL.Replace("\\", "/"),
                    CreatedAt = x.CreatedAt,
                })));

        }
    }
}
