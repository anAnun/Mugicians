using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class GoogleLogInRequest
    {
        [Required]
        public string GoogleToken { get; set; }
    }
}