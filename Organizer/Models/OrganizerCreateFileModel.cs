using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class OrganizerCreateFileModel
    {
        public string AudioFile { get; set; }
        public string Description { get; set; }
        public int SongId { get; set; }
    }
}