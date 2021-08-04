using System;

namespace Visit.DataAccess.Models
{
    public partial class UserNotification
    {
        public int NotificationId { get; set; }
        public string FkUserId { get; set; }
        public int FkPostId { get; set; }
        public DateTime DatetimeOfNot { get; set; }
        public int? PostCommentId { get; set; }
        public int? LikeId { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual User FkUser { get; set; }
        public virtual Like Like { get; set; }
        public virtual PostComment PostComment { get; set; }
    }
}
