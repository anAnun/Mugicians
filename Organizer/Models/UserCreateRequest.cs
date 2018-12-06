using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserCreateRequest
    {
        [Required]
        [MaxLength(50, ErrorMessage = "User names can only be 50 characters in length.")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(254, ErrorMessage = "Emails can only be 254 characters in length.")]
        public string Email { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Passwords can only be 50 characters in length.")]
        public string Password { get; set; }

        [Required]
        public int UserTypeId { get; set; }
    }
}