using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Theatre.DataProcessor.ImportDto
{
    public class ImportTtheatersTicketsDto
    {
        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Name { get; set; }

        [Range(1, 10)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Director { get; set; }

        public List<TicketDto> Tickets { get; set; }
    }

    public class TicketDto
    {
        [Range(1.00, 100.00)]
        public decimal Price { get; set; }

        [Range(1, 10)]
        public sbyte RowNumber { get; set; }

        public int PlayId { get; set; }
    }
}
