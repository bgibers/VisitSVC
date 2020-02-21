using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class User
    {
        public User()
        {
            PostComments = new HashSet<PostComments>();
            Posts = new HashSet<Posts>();
            UserFollowingFkFollowUser = new HashSet<UserFollowing>();
            UserFollowingFkMainUser = new HashSet<UserFollowing>();
            UserLocations = new HashSet<UserLocations>();
            UserMessagesFkReciverUser = new HashSet<UserMessages>();
            UserMessagesFkSenderUser = new HashSet<UserMessages>();
            UserTags = new HashSet<UserTags>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avi { get; set; }
        public int? FkBirthLocationId { get; set; }
        public int? FkResidenceLocationId { get; set; }

        public virtual Location FkBirthLocation { get; set; }
        public virtual Location FkResidenceLocation { get; set; }
        public virtual ICollection<PostComments> PostComments { get; set; }
        public virtual ICollection<Posts> Posts { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingFkFollowUser { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingFkMainUser { get; set; }
        public virtual ICollection<UserLocations> UserLocations { get; set; }
        public virtual ICollection<UserMessages> UserMessagesFkReciverUser { get; set; }
        public virtual ICollection<UserMessages> UserMessagesFkSenderUser { get; set; }
        public virtual ICollection<UserTags> UserTags { get; set; }
    }
}
