namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var seliarizer = new XmlSerializer(typeof(ImportPlaysDto[]), new XmlRootAttribute("Plays"));

            var reader = new StringReader(xmlString);
            var playsDto = (ImportPlaysDto[])seliarizer.Deserialize(reader);
            reader.Close();

            List<Play> plays = new List<Play>();
            StringBuilder sb = new StringBuilder();

            foreach (var playDto in playsDto)
            {
                bool isCurrentDTOGenreValid = Enum.TryParse(playDto.Genre, out Genre genre);
                bool isCurrentDTODurationValid = TimeSpan
                    .TryParse(playDto.Duration, CultureInfo.InvariantCulture, out TimeSpan duration);

                if (!IsValid(playDto) || !isCurrentDTODurationValid || !isCurrentDTOGenreValid || duration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var play = new Play
                {
                    Title = playDto.Title,
                    Description = playDto.Description,
                    Rating = playDto.Rating,
                    Genre = genre,
                    Duration = duration,
                    Screenwriter = playDto.Screenwriter
                };

                plays.Add(play);
                sb.AppendLine($"Successfully imported {play.Title} with genre {playDto.Genre} and a rating of {play.Rating}!");
            }

            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var seliarizer = new XmlSerializer(typeof(ImportCastsDto[]), new XmlRootAttribute("Casts"));

            var reader = new StringReader(xmlString);
            var castsDto = (ImportCastsDto[])seliarizer.Deserialize(reader);
            reader.Close();

            List<Cast> casts = new List<Cast>();
            StringBuilder sb = new StringBuilder();

            foreach (var castDto in castsDto)
            {
               
                if (!IsValid(castDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var cast = new Cast
                {
                    FullName = castDto.FullName,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PhoneNumber = castDto.PhoneNumber,
                    PlayId = castDto.PlayId
                };

                casts.Add(cast);
                string mainOrNot = cast.IsMainCharacter ? "main" : "lesser";
                sb.AppendLine($"Successfully imported actor {cast.FullName} as a {mainOrNot} character!");
            }

            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var theatersDto = JsonConvert.DeserializeObject<IEnumerable<ImportTtheatersTicketsDto>>(jsonString);

            List<Theatre> theatres = new List<Theatre>();
            StringBuilder sb = new StringBuilder();

            foreach (var theatreDto in theatersDto)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre
                {
                    Director = theatreDto.Director,
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                };

                foreach (var ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    theatre.Tickets.Add(new Ticket
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId
                    });
                }

                theatres.Add(theatre);
                sb.AppendLine($"Successfully imported theatre {theatre.Name} with #{theatre.Tickets.Count} tickets!");
            }

            context.Theatres.AddRange(theatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
