using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserUpdateBasicInfoRequest
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string AvatarUrl { get; set; }
    }
}