using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Security
{
    public class SimpleClaim
    {
        public string Type { get; set; }

        public string Value { get; set; }


        public SimpleClaim(string type, string value)
        {
            Type = type;
            Value = value;
        }


    }
}