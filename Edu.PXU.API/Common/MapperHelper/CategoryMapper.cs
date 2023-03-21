using AutoMapper;
using Edu.PXU.API.App;
using Edu.PXU.API.Model.Category;
using Edu.PXU.EntityFECore.Entity;

namespace Edu.PXU.API.Common.MapperHelper
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryReponseDto>()
                .ReverseMap();

            CreateMap<Category, CategoryRequestDto>()
                .ReverseMap();
        }
    }
}
