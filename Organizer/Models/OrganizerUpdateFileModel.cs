using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class OrganizerUpdateFileModel : OrganizerCreateFileModel
    {
        [Required]
        public int? Id { get; set; }
    }
}