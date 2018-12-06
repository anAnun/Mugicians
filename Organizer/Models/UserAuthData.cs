using Organizer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class UserAuthData : IUserAuthData
    {
        public int Id
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public IEnumerable<string> Roles
        {
            get; set;
        }
    }
}