using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class UserFollowing
    {
        public int FollowUserId { get; set; }
        public int? FkMainUserId { get; set; }
        public int? FkFollowUserId { get; set; }
        public DateTime? FollowSince { get; set; }

        public virtual User FkFollowUser { get; set; }
        public virtual User FkMainUser { get; set; }
    }
}
