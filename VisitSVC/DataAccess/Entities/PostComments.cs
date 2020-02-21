using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class PostComments
    {
        public int Id { get; set; }
        public int? FkPostId { get; set; }
        public string CommentText { get; set; }
        public int? FkUserIdOfCommenting { get; set; }
        public DateTime? DatetimeOfComments { get; set; }

        public virtual Posts FkPost { get; set; }
        public virtual User FkUserIdOfCommentingNavigation { get; set; }
    }
}
