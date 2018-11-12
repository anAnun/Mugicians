using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class SongFileModel
    {
        public int Id { get; set; }
        public string AudioFile { get; set; }
        public int SongId { get; set; }
        public string Description { get; set; }
    }
}