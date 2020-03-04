using System;

namespace Visit.DataAccess.Models
{
    public partial class UserMessage
    {
        public int UserMessageId { get; set; }
        public string FkSenderUserId { get; set; }
        public string FkRecieverUserId { get; set; }
        public string MessageContent { get; set; }
        public DateTime? MessageSentTime { get; set; }

        public virtual User FkRecieverUser { get; set; }
        public virtual User FkSenderUser { get; set; }
    }
}
