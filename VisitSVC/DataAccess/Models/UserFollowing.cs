using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class UserFollowing
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserFollowingId { get; set; }
        public int? FkMainUserId { get; set; }
        public int? FkFollowUserId { get; set; }
        public DateTime? FollowSince { get; set; }

        public virtual User FkFollowUser { get; set; }
        public virtual User FkMainUser { get; set; }
    }
}
