namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context
                .Theatres
                .Where(h => h.NumberOfHalls >= numbersOfHalls && h.Tickets.Count >= 20)
                .ToList()
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(e => e.RowNumber >= 1 && e.RowNumber <= 5)
                        .Sum(s => s.Price),
                    Tickets = t.Tickets
                        .Where(e => e.RowNumber >= 1 && e.RowNumber <= 5)
                        .Select(e => new
                        {
                            Price = e.Price,
                            RowNumber = e.RowNumber
                        })
                        .OrderByDescending(p => p.Price)
                        .ToList()

                })
                .OrderByDescending(n => n.Halls)
                .ThenBy(n => n.Name)
                .ToList();


            var writer = new StringWriter();

            return JsonConvert.SerializeObject(theatres, Formatting.Indented);

        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context
                .Plays
                .Where(w => w.Rating <= rating)
                .ToList()
                .Select(p => new ExportPlaysDto()
                {
                    Title = p.Title,
                    Duration = p.Duration.ToString("c"),
                    Rating = p.Rating == 0f ? "Premier" : p.Rating.ToString(),
                    Genre = p.Genre.ToString(),
                    Actors = p.Casts
                        .Where(e => e.IsMainCharacter)
                        .Select(a => new ActorDto()
                        {
                            FullName = a.FullName,
                            MainCharacter = $"Plays main character in '{p.Title}'."
                        })
                        .OrderByDescending(a => a.FullName)
                        .ToArray(),
                })
                .OrderBy(t => t.Title)
                .ThenByDescending(g => g.Genre)
                .ToArray();

            XmlSerializer seliarizer = new XmlSerializer(typeof(ExportPlaysDto[]), new XmlRootAttribute("Plays"));

            StringWriter witer = new StringWriter();

            XmlSerializerNamespaces xmlns = new XmlSerializerNamespaces();
            xmlns.Add("", "");

            seliarizer.Serialize(witer, plays, xmlns);

            witer.Close();

            return witer.ToString().TrimEnd();
        }
    }
}
