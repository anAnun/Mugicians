using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class OrganizerUpdateModel : OrganizerCreateModel
    {
        [Required]
        public int? Id { get; set; }
    }
}