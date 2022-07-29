using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using TeisterMask.DataProcessor.ExportDto;

namespace TeisterMask.DataProcessor
{
    using System;

    using Data;

    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute root = new XmlRootAttribute("Projects");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportProjectDto[]), root);

            XmlSerializerNamespaces nameSpases = new XmlSerializerNamespaces();
            nameSpases.Add(string.Empty, string.Empty);

            using StringWriter sw = new StringWriter(sb);

            ExportProjectDto[] exportProjects = context
                .Projects
                .ToArray()
                .Where(p => p.Tasks.Any())
                .Select(p => new ExportProjectDto()
                {
                    Name = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate.HasValue ? "Yes" : "No",
                    Tasks = p.Tasks.Select(t => new ExportProjectTaskDto()
                        {
                            TaskName = t.Name,
                            Label = t.LabelType.ToString()
                        })
                        .OrderBy(t=>t.TaskName)
                        .ToArray()
                })
                .OrderByDescending(p=>p.TasksCount)
                .ThenBy(p=>p.Name)
                .ToArray();

            xmlSerializer.Serialize(sw, exportProjects, nameSpases);

            return sb.ToString().TrimEnd();
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
                        .Select(et => et.Task)
                        .OrderByDescending(t => t.DueDate)
                        .ThenBy(t => t.Name)
                        .Select(s => new
                        {
                            TaskName = s.Name,
                            OpenDate = s.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = s.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = s.LabelType.ToString(),
                            ExecutionType = s.ExecutionType.ToString()
                        })
                        .ToArray()


                })
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(u => u.Username)
                .Take(10)
                .ToArray();

            string json = JsonConvert.SerializeObject(mostBusiestEmployee, Formatting.Indented);

            return json;
        }
    }
}