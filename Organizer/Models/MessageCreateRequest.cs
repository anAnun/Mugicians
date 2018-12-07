using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class MessageCreateRequest
    {
        public string Message { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}