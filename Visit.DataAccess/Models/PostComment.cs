using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visit.DataAccess.Models
{
    public partial class PostComment
    {
        public PostComment()
        {
            UserNotification = new HashSet<UserNotification>();
        }

        public int PostCommentId { get; set; }
        public string FkUserIdOfCommenting { get; set; }
        public int FkPostId { get; set; }
        public string CommentText { get; set; }
        public DateTime DatetimeOfComments { get; set; }
        public sbyte? Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted
        {
            get => Deleted > 0;
            set { this.Deleted = (sbyte)(value ? 1 : 0);  }
        }
        
        public virtual Post FkPost { get; set; }
        public virtual User FkUserIdOfCommentingNavigation { get; set; }
        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }
}
