using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Post = new HashSet<Post>();
            PostComment = new HashSet<PostComment>();
            UserFollowingFkFollowUser = new HashSet<UserFollowing>();
            UserFollowingFkMainUser = new HashSet<UserFollowing>();
            UserLocation = new HashSet<UserLocation>();
            UserMessageFkRecieverUser = new HashSet<UserMessage>();
            UserMessageFkSenderUser = new HashSet<UserMessage>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingFkFollowUser { get; set; }
        public virtual ICollection<UserFollowing> UserFollowingFkMainUser { get; set; }
        public virtual ICollection<UserLocation> UserLocation { get; set; }
        public virtual ICollection<UserMessage> UserMessageFkRecieverUser { get; set; }
        public virtual ICollection<UserMessage> UserMessageFkSenderUser { get; set; }
    }
}
