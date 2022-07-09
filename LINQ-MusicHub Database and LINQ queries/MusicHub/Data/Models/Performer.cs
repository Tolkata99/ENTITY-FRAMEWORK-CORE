using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using MusicHub.Common;

namespace MusicHub.Data.Models
{
    public class Performer
    {
        public Performer()
        {
            this.PerformerSongs = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstans.MaxLenghtFirstName)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(GlobalConstans.MaxLenghtLastName)]
        public string LastName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public decimal NetWorth { get; set; }

        public virtual ICollection<SongPerformer> PerformerSongs { get; set; }

    }
}
