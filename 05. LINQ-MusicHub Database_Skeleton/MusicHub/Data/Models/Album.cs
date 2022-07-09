
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using MusicHub.Common;
using MusicHub.MusicHub.Data.MusicHub.Data.Models;

namespace MusicHub.Data.Models
{
    public class Album
    {
        public Album()
        {
            this.Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstans.MaxLenghtAlbumName)]
        public string Name { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [NotMapped]
        public decimal Price 
            =>this.Songs.Count > 0 ? this.Songs.Sum(p => p.Price) : 0;


        [ForeignKey(nameof(Producer))]
        public int? ProducerId { get; set; }
        public Producer Producer { get; set; }

        public virtual ICollection<Song> Songs { get; set; }
    }
}
