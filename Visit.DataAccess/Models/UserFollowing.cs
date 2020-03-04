using System;

namespace Visit.DataAccess.Models
{
    public partial class UserFollowing
    {
        public int UserFollowingId { get; set; }
        public string FkMainUserId { get; set; }
        public string FkFollowUserId { get; set; }
        public DateTime? FollowSince { get; set; }

        public virtual User FkFollowUser { get; set; }
        public virtual User FkMainUser { get; set; }
    }
}
