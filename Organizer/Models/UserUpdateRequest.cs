using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserUpdateRequest
    {
        [Required]
        public int? Id { get; set; }
        [MaxLength(50)]
        public string UserName { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
    }
}