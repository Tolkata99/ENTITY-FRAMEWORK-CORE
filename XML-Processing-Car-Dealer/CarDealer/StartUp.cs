using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Xml.Serialization;
using CarDealer.Dtos.Export;
using CarDealer.Dtos.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CarDealer
{
    using Data;
    using System;
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext dbContext = new CarDealerContext();
            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //Console.WriteLine("Database successfully created!");

            //string xmlInput = File.ReadAllText(@"../../../Datasets\sales.xml");

            Console.WriteLine(GetCarsWithDistance(dbContext));

        }
        //Export

        public static string GetCarsWithDistance(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();

            ExportCarWithDistanceDto[] exportCarDistanceDto = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(m => m.Make)
                .ThenBy(m => m.Model)
                .Take(10)
                .Select(dto => new ExportCarWithDistanceDto()
                {
                    Model = dto.Model,
                    Make = dto.Make,
                    TravelledDistance = dto.TravelledDistance
                })
                .ToArray();

            XmlRootAttribute root = new XmlRootAttribute("cars");
            XmlSerializer xmlDeSerializer = new XmlSerializer(typeof(ExportCarWithDistanceDto[]), root);

            using StringWriter writer = new StringWriter(sb);

            xmlDeSerializer.Serialize(writer, exportCarDistanceDto);


            return sb.ToString().TrimEnd();
        }
        //Export

        //Import
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSalesDto[]), root);

            StringReader reader = new StringReader(inputXml); //using?
            ImportSalesDto[] salesDtos = (ImportSalesDto[])xmlSerializer
                .Deserialize(reader);

            ICollection<Sale> sales = new List<Sale>();

            foreach (var sale in salesDtos)
            {
                if (!context.Cars.Any(i => i.Id == sale.CarId))
                {
                    continue;
                }

                Sale salee = new Sale()
                {
                    Discount = sale.Discount,
                    CustomerId = sale.CustomerId,
                    CarId = sale.CarId
                };


                sales.Add(salee);
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();


            return $"Successfully imported {sales.Count}";
        }

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Customers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomersDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            ImportCustomersDto[] customerDtos = (ImportCustomersDto[])xmlSerializer
                .Deserialize(reader);

            Customer[] customers = customerDtos
                .Select(dto => new Customer()
                {
                    Name = dto.Name,
                    BirthDate = dto.birthDate,
                    IsYoungDriver = dto.isYoungDriver

                })
                .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }

        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Cars");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCarsDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            ImportCarsDto[] carDtos = (ImportCarsDto[])xmlSerializer
                .Deserialize(reader);

            ICollection<Car> cars = new List<Car>();
            foreach (var dto in carDtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TravelledDistance = dto.TraveledDistance
                };

                ICollection<PartCar> carPart = new List<PartCar>();
                var dtos = dto.Parts.Select(p => p.Id).Distinct();
                foreach (var partId in dtos)
                {
                    if (!context.Parts.Any(p => p.Id == partId))
                    {
                        continue;
                    }

                    carPart.Add(new PartCar()
                    {
                        Car = car,
                        PartId = partId
                    });
                }

                car.PartCars = carPart;
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}"; ;
        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartsDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            ImportPartsDto[] partsDtos = (ImportPartsDto[])xmlSerializer
                .Deserialize(reader);

            var supplier = context.Suppliers.Select(e => e.Id).ToArray();

            Part[] parts = partsDtos
                .Where(e => supplier.Contains(e.SupplierId))
                .Select(dto => new Part()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId,
                })
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Suppliers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            ImportSupplierDto[] supplierDtos = (ImportSupplierDto[])xmlSerializer
                 .Deserialize(reader);


            Supplier[] suppliers = supplierDtos
                .Select(Dto => new Supplier()
                {
                    Name = Dto.Name,
                    IsImporter = Dto.IsImporter

                })
                .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
        //Import
    }
}