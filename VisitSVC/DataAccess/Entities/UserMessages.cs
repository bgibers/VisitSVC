using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class UserMessages
    {
        public int UserMessages1 { get; set; }
        public int? FkSenderUserId { get; set; }
        public int? FkReciverUserId { get; set; }
        public string MessageContent { get; set; }
        public DateTime? MessageSentTime { get; set; }

        public virtual User FkReciverUser { get; set; }
        public virtual User FkSenderUser { get; set; }
    }
}
