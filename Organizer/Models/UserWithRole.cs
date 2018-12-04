using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserWithRole : User
    {
        public int? Role {get;set;}
        public string UserTypeName { get; set; }
    }
}