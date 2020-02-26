using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class PostComment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PostCommentId { get; set; }
        public int? FkUserIdOfCommenting { get; set; }
        public int? FkPostId { get; set; }
        public string CommentText { get; set; }
        public DateTime? DatetimeOfComments { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual User FkUserIdOfCommentingNavigation { get; set; }
    }
}
