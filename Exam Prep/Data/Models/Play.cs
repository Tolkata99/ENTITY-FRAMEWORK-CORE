using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            Casts = new HashSet<Cast>();
            Tickets = new HashSet<Ticket>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(50)]
        public string Title { get; set; }

        [DisplayFormat(DataFormatString = @"{0:hh\:mm\:ss}")]
        public TimeSpan Duration  { get; set; }

        [Range(0.00, 10.00)]
        public float Rating { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [MaxLength(700)]
        public string Description  { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Screenwriter { get; set; }

        public ICollection<Cast> Casts { get; set; }

        public ICollection<Ticket> Tickets { get; set; }
    }
}
