using AutoMapper;

namespace EcommerceApi.Models.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductModel, ProductModelDto>().ReverseMap();               
        }

      
    }
}
