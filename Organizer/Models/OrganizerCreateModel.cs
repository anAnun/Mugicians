using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class OrganizerCreateModel
    {
        [Required]
        [MaxLength(50)]
        public string SongName { get; set; }

        public string Lyrics { get; set; }

    }
}