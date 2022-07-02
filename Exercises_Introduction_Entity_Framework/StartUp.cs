using Exercises_Introduction_Entity_Framework.Data.Models;
using System;
using System.Linq;
using System.Text;

namespace Exercises_Introduction_Entity_Framework
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            SoftUniContext sofContex = new SoftUniContext();
            Console.WriteLine(GetEmployeesFromResearchAndDevelopment(sofContex));
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var dptEmployee = context
                .Employees
                .Where(e => e.Department.Name == "Research and Development")
                .OrderBy(e => e.Salary)
                .OrderByDescending(f => f.FirstName)
                .ToArray();

            foreach (var dptEmpl in dptEmployee)
            {
                sb.AppendLine($"{dptEmpl.FirstName} {dptEmpl.LastName} from Research and Development - ${dptEmpl.Salary:F2}");
            }

            return sb.ToString();
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
    }


}
