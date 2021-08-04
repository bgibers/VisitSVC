using System;
using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Like = new HashSet<Like>();
            Post = new HashSet<Post>();
            PostComment = new HashSet<PostComment>();
            UserFollowingFkFollowUser = new HashSet<UserFollowing>();
            UserFollowingFkMainUser = new HashSet<UserFollowing>();
            UserLocation = new HashSet<UserLocation>();
            UserMessageFkRecieverUser = new HashSet<UserMessage>();
            UserMessageFkSenderUser = new HashSet<UserMessage>();
            UserNotificationFkUser = new HashSet<UserNotification>();
            UserNotificationFkUserWhoNotifiedNavigation = new HashSet<UserNotification>();
        }

        public string Id { get; set; }
        public string FcmToken { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Title { get; set; }
        public string Education { get; set; }
        public string BirthLocation { get; set; }
        public string ResidenceLocation { get; set; }
        public string Avi { get; set; }
        public int? FkBirthLocationId { get; set; }
        public int? FkResidenceLocationId { get; set; }

        public virtual Location FkBirthLocation { get; set; }
        public virtual Location FkResidenceLocation { get; set; }
        public virtual ICollection<Like> Like { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingFkFollowUser { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingFkMainUser { get; set; }
        public virtual ICollection<UserLocation> UserLocation { get; set; }
        public virtual ICollection<UserMessage> UserMessageFkRecieverUser { get; set; }
        public virtual ICollection<UserMessage> UserMessageFkSenderUser { get; set; }
        public virtual ICollection<UserNotification> UserNotificationFkUser { get; set; }
        public virtual ICollection<UserNotification> UserNotificationFkUserWhoNotifiedNavigation { get; set; }
    }
}
