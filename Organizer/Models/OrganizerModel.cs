using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class OrganizerModel
    {
        public int Id { get; set; }
        public string SongName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Lyrics { get; set; }
    }
}