

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;
using MusicHub.Common;
using MusicHub.Data.Models;
using MusicHub.Data.Models.Enums;

namespace MusicHub.MusicHub.Data.MusicHub.Data.Models
{
    public class Song
    {
        public Song()
        {
            this.SongPerformers = new HashSet<SongPerformer>();
        }

        [Key]
        public int Id { get; set; }

        [MaxLength(GlobalConstans.MaxLenghtSongName)]
        [Required]
        public string Name { get; set; }

        [Required]
        public TimeSpan Duration { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [ForeignKey(nameof(Album))]
        public int? AlbumId { get; set; }
        public virtual Album Album { get; set; }

        [Required]
        [ForeignKey(nameof(Writer))]
        public int WriterId { get; set; }
        public virtual Writer Writer { get; set; }

        [Required]
        public decimal Price { get; set; }

        public virtual ICollection<SongPerformer> SongPerformers { get; set; }

    }
}
