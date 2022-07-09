
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MusicHub.Common;
using MusicHub.MusicHub.Data.MusicHub.Data.Models;

namespace MusicHub.Data.Models
{
    public class Writer
    {
        public Writer()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(GlobalConstans.MaxLenghtWriter)]
        public string Name { get; set; }

        public string Pseudonym { get; set; }

        public virtual ICollection<Song> Songs { get; set; }

    }
}
