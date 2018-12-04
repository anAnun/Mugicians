using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int? UserTypeId { get; set; }
    }
}