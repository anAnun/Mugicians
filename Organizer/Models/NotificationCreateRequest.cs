using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class NotificationCreateRequest
    {
        [Required]
        public int? UserId { get; set; }

        [Required]
        public string NotificationJson { get; set; }
    }
}