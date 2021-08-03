using System;
using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public partial class Like
    {
        public Like()
        {
            UserNotification = new HashSet<UserNotification>();
        }

        public int LikeId { get; set; }
        public int FkPostId { get; set; }
        public string FkUserId { get; set; }
        public DateTime? TimeOfLike { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }
}
