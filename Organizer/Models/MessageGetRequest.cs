using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class MessageGetRequest
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public int ReceiverId { get; set; }
        public string ReceiverName { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime DateCreated { get; set; }
    }
}