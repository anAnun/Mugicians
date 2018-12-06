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
        public string FirstName { get; set; }
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(254)]
        public string Email { get; set; }
        public int? UserTypeId { get; set; }
        [MaxLength(250)]
        public string AvatarUrl { get; set; }
        public bool? SubscribeToNewsletter { get; set; }
    }
}