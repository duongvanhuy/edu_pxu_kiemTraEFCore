using AutoMapper;
using Edu.PXU.API.Model.Product;
using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.Common.MapperHelper
{
    public class ProductMapper : Profile
    {
        public ProductMapper() {

            CreateMap<Product, ProductReponseDto>()
                .ForMember(d => d.FileName, o => o.MapFrom(src => src.ProductImages.Select(x => x.Image).Select(x => x.FileName).ToList()))
                .ReverseMap();

            CreateMap<Product, ProductRequestDto>()
               
               .ReverseMap();

            CreateMap<Product, ProductUpdateDto>()

            .ReverseMap();
        }
    }
}
