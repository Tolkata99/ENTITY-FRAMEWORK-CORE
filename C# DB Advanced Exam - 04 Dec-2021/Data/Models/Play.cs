using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Castle.DynamicProxy.Generators.Emitters;
using Theatre.Common;
using Theatre.Data.Models.Enums;

namespace Theatre.Data.Models
{
    public class Play
    {
        public Play()
        {
            this.Tickets = new HashSet<Ticket>();
            this.Casts = new HashSet<Cast>();

        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_TITLE_MAX_LENGHT)]
        public string Title { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public float Rating { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_DESCRIPTION_MAX_LENGHT)]
        public string Description { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PLAY_SCREENWRITER_MAX_LENGHT)]
        public string Screenwriter { get; set; }

        public virtual ICollection<Cast> Casts { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
