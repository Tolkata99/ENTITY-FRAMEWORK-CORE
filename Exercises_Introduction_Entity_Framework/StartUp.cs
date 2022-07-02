using Exercises_Introduction_Entity_Framework.Data.Models;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercises_Introduction_Entity_Framework
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using(var dbContext = new SoftUniContext())
            {
                SoftUniContext context = new SoftUniContext();
                Console.WriteLine(GetEmployee147(context));
            }
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

        //Problem 7

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context
                .Employees
                .Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year >= 2001
                                                       && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    AllProjects = e.EmployeesProjects
                                   .Select(ep => new
                                   {
                                       ProjectName = ep.Project.Name,
                                       ProjectStart = ep
                                                       .Project
                                                       .StartDate
                                                       .ToString("M/d/yyyy h:mm:ss tt"),
                                       ProjectEnd = ep
                                                       .Project
                                                       .EndDate
                                                       .HasValue ? ep
                                                                     .Project
                                                                     .EndDate
                                                                     .Value
                                                                     .ToString("M/d/yyyy h:mm:ss tt") : "not finished"
                                   })
                                   .ToArray()

                })
                .ToArray();

            foreach (var emplWitManager in employees)
            {
                sb.AppendLine($"{emplWitManager.FirstName} {emplWitManager.LastName} - Manager: " +
                    $"{emplWitManager.ManagerFirstName} {emplWitManager.ManagerLastName} ");

                foreach (var projects in emplWitManager.AllProjects)
                {
                    sb.AppendLine($"-- {projects.ProjectName} - {projects.ProjectStart} - {projects.ProjectEnd} ");
                }

            }

            return sb.ToString().TrimEnd();
        }

        //Problem 8

        public static string GetAddressesByTown(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context
                .Addresses
                .OrderByDescending(a => a.Employees.Count)
                .ThenBy(a => a.TownId)
                .ThenBy(at=>at.AddressText)
                .Take(10)
                .Select(s => new
                {
                    Text = s.AddressText,
                    Town = s.Town.Name,
                    count = s.Employees.Count
                })
                .ToArray();

            foreach (var infoAd in addresses)
            {
                sb.AppendLine($"{infoAd.Text}, {infoAd.Town} - {infoAd.count} employees");
            }

            return sb.ToString().TrimEnd();
        }

        //Problem 9
        public static string GetEmployee147(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee = context
                .Employees
                .Where(y=>y.EmployeeId == 147)
                .Select(t => new
                {
                    t.FirstName,
                    t.LastName,
                    t.JobTitle,
                    AllProjects = t.EmployeesProjects
                                   .Select(y => y.Project.Name)
                                   .OrderBy(y => y)
                                   .ToArray()

                })
                .First();

          




            sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
            foreach (var project in employee.AllProjects)
            {
                sb.AppendLine(project);
            }

            return sb.ToString().TrimEnd();

        }
    }


}
