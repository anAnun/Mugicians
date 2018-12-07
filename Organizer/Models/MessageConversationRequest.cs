using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organizer.Models
{
    public class MessageConversationRequest
    {
        public int OtherUserId { get; set; }
        public string OtherUserName { get; set; }
        public int UnreadMsgCount { get; set; }
        public DateTime LastMsgDate { get; set; }
    }
}