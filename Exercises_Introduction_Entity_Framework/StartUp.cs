﻿using Exercises_Introduction_Entity_Framework.Data.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercises_Introduction_Entity_Framework
{
    public class StartUp
    {
        public static async Task Main(string[] args)
        {
            using SoftUniContext sofContex = new SoftUniContext();
            Console.WriteLine(await AddNewAddressToEmployee(sofContex));
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var dptEmployee = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(n => new
                {
                    n.FirstName,
                    n.Salary,
                    n.LastName,
                    n.Department.Name
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(f => f.FirstName)
                .ToArray();

            foreach (var dptEmpl in dptEmployee)
            {
                sb.AppendLine($"{dptEmpl.FirstName} {dptEmpl.LastName} from Research and Development - ${dptEmpl.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Select(e => new
                {
                    e.Salary,
                    e.FirstName
                })
                .Where(e => e.Salary > 50000)
                .OrderBy(e => e.FirstName)
                .ToArray();
                
            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString();
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var allEmployees = context
                .Employees
                .Select(e=> new
                {
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.Salary,
                    e.JobTitle
                })
                .ToArray();

            foreach (var employee in allEmployees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.MiddleName} {employee.LastName} {employee.JobTitle} {employee.Salary:f2}");
            }

            return sb.ToString();
                
        }

        public static async Task<string> AddNewAddressToEmployee(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();
           
            Address adress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4
            };
            context.Addresses.Add(adress);

            Employee nakov = context
                .Employees
                .FirstOrDefault(c => c.LastName == "Nakov");

            nakov.Address = adress;
           await context.SaveChangesAsync();


            string[] allEmpl = context
                .Employees
                .OrderByDescending(n => n.AddressId)
                .Take(10)
                .Select(e => e.Address.AddressText)
                .ToArray();

            foreach (var empl in allEmpl)
            {
               
                sb.AppendLine(empl);
            }

            return sb.ToString();

        }
    }


}
