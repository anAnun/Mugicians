using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string NotificationJson { get; set; }
    }
}