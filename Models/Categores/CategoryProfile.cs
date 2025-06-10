using AutoMapper;

namespace EcommerceApi.Models.Categores
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryModel, CategoryModelDto>().ReverseMap();
        }

    }
}
