using System;

namespace Visit.DataAccess.Models
{
    public class PostComment
    {
        public int PostCommentId { get; set; }
        public string FkUserIdOfCommenting { get; set; }
        public int? FkPostId { get; set; }
        public string CommentText { get; set; }
        public DateTime? DatetimeOfComments { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual User FkUserIdOfCommentingNavigation { get; set; }
    }
}