using System;

namespace Visit.DataAccess.Models
{
    public class UserFollowing
    {
        /// <summary>
        /// Primary key mapping
        /// </summary>
        public int UserFollowingId { get; set; }
        
        /// <summary>
        /// The id of the user being followed
        /// </summary>
        public string FkMainUserId { get; set; }
        
        /// <summary>
        /// The id of the user following
        /// </summary>
        public string FkFollowUserId { get; set; }
        
        /// <summary>
        /// The date the user started following
        /// </summary>
        public DateTime? FollowSince { get; set; }

        /// <summary>
        /// The follower
        /// </summary>
        public virtual User FkFollowUser { get; set; }
        
        /// <summary>
        /// The user being followed
        /// </summary>
        public virtual User FkMainUser { get; set; }
    }
}