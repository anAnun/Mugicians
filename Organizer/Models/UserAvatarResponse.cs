using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserAvatarResponse
    {
        public int Id { get; set; }
        public string AvatarUrl { get; set; }
        public int UserTypeId { get; set; }
        public string FullName { get; set; }
    }
}