using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace TeisterMask.DataProcessor
{
    using System;

    using Data;

    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            throw new NotImplementedException();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var mostBusiestEmployee = context
                .Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username.ToString(),
                    Tasks = e.EmployeesTasks
                        .Where(et => et.Task.DueDate >= date)
                        .Select(et=>et.Task)
                        .OrderByDescending(t=>t.DueDate)
                        .ThenBy(t=>t.Name)
                        .Select(s => new
                        {
                            TaskName = s.Name,
                            OpenDate = s.OpenDate.ToString("d",CultureInfo.InvariantCulture),
                            DueDate = s.DueDate.ToString("d",CultureInfo.InvariantCulture),
                            LabelType = s.LabelType.ToString(),
                            ExecutionType = s.ExecutionType.ToString()
                        })
                        .ToArray()


                })
                .OrderByDescending(e=>e.Tasks.Length)
                .ThenBy(u=>u.Username)
                .Take(10)
                .ToArray();

            string json = JsonConvert.SerializeObject(mostBusiestEmployee, Formatting.Indented);

            return json;
        }
    }
}