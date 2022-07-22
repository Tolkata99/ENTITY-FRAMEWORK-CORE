using System.IO;
using System.Linq;
using System.Threading.Channels;
using System.Xml.Serialization;
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

            string xmlInput = File.ReadAllText(@"../../../Datasets\parts.xml");

            Console.WriteLine(ImportParts(dbContext, xmlInput));

        }

        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute root = new XmlRootAttribute("Parts");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartsDto[]), root);

            using StringReader reader = new StringReader(inputXml);
            ImportPartsDto[] partsDtos = (ImportPartsDto[])xmlSerializer
                .Deserialize(reader);

            var supplier = context.Suppliers.Select(e=>e.Id).ToArray();

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
    }
}