using AutoMapper;
using ProductShop.Dtos.InputModel;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserInputDto, User>();
            CreateMap<ProductsInputDto, Product>();
            CreateMap<CategoriesInputDto, Category>();
            CreateMap<CategoryProductInputDto, CategoryProduct>();
        }
    }
}
