using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class UserMessage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserMessageId { get; set; }
        public int? FkSenderUserId { get; set; }
        public int? FkRecieverUserId { get; set; }
        public string MessageContent { get; set; }
        public DateTime? MessageSentTime { get; set; }

        public virtual User FkRecieverUser { get; set; }
        public virtual User FkSenderUser { get; set; }
    }
}
