using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Castle.Core.Internal;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Dtos.InputModel;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();

            context.Database.EnsureCreated();
            context.Database.EnsureDeleted();

            var usersJsonAsString = File
                .ReadAllText("../../../Datasets/users.json");

            string productsJsonString = File
                .ReadAllText("../../../Datasets/products.json");

            var categoriesJsonAsString = File
                .ReadAllText("../../../Datasets/categories.json");

            var categoriesProductsJsonAsString = File
                .ReadAllText("../../../Datasets/categories-products.json");



            Console.WriteLine(GetProductsInRange(context));
        }

        public static string GetProductsInRange(ProductShopContext context)
        {

            var result = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(x=>x.Price)
                .Select(x=> new
                {
                    Seller = $"{x.Seller.FirstName} {x.Seller.LastName}",
                    x.Price,
                    x.Name
                })
                .ToArray();

            var export = JsonConvert.SerializeObject(result);

            return export;
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoriesInputDto> categories = JsonConvert
                            .DeserializeObject<IEnumerable<CategoriesInputDto>>(inputJson)
                            .Where(x=> !string.IsNullOrEmpty(x.Name));

            IMapper mapper = MapperInitialize();

            var mapperCategories = mapper.Map<IEnumerable<CategoryProduct>>(categories);

            context.AddRange(mapperCategories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count()}";

        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<CategoryProductInputDto> categoryProduct = JsonConvert
                .DeserializeObject<IEnumerable<CategoryProductInputDto>>(inputJson);
            
            IMapper mapper = MapperInitialize();

            var mapperCategoriesProd = mapper.Map<IEnumerable<Category>>(categoryProduct);

            context.AddRange(mapperCategoriesProd);
            context.SaveChanges();

            return $"Successfully imported {categoryProduct.Count()}";
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            IEnumerable<UserInputDto> users = JsonConvert.DeserializeObject<IEnumerable<UserInputDto>>(inputJson);

           IMapper mapper = MapperInitialize();

            //IMapper mapper = new Mapper(mapperConfig);

            IEnumerable<User> mappedUsers = mapper.Map<IEnumerable<User>>(users);

            //  IEnumerable<User> mappingUser = users.Select(x => x.MapToDomainUser()).ToList();

            context.Users.AddRange(mappedUsers);
            context.SaveChanges();


            return $"Successfully imported {mappedUsers.Count()}";

        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            IEnumerable<ProductsInputDto> products = JsonConvert.DeserializeObject<IEnumerable<ProductsInputDto>>(inputJson);

            MapperInitialize();

            IEnumerable<Product> mappedProduct = Mapper.Map<IEnumerable<Product>>(products);

            context.Products.AddRange(mappedProduct);
            context.SaveChanges();


            return  $"Successfully imported {mappedProduct.Count()}";

        }

        public static IMapper MapperInitialize()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(mapperConfig);

            return mapper;
        }
            
    }


    public static class UserMapping
    {
        public static User MapToDomainUser(this UserInputDto userDto)
        {
            return new User()
            {
                Age = userDto.Age,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName

            };
        }
    }
}