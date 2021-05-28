using System;
using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public sealed class User
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
        }
        
        public string Id { get; set; }
        
        public string FcmToken { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public DateTime? Birthday { get; set; }
        public string Title { get; set; }
        public string Education { get; set; }
        
        /// <summary>
        /// This is temporary. Ideally we want to use the FK
        /// </summary>
        public string BirthLocation { get; set; }
        
        /// <summary>
        /// This is temporary. Ideally we want to use the FK
        /// </summary>
        public string ResidenceLocation { get; set; }
        public string Avi { get; set; }
        public int? FkBirthLocationId { get; set; }
        public int? FkResidenceLocationId { get; set; }
        public Location FkBirthLocation { get; set; }
        public Location FkResidenceLocation { get; set; }
        public ICollection<Like> Like { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<PostComment> PostComment { get; set; }
        
        /// <summary>
        ///  All people user is following
        /// </summary>
        public ICollection<UserFollowing> UserFollowingFkFollowUser { get; set; }
        
        /// <summary>
        /// Everyone following this user
        /// </summary>
        public ICollection<UserFollowing> UserFollowingFkMainUser { get; set; }
        public ICollection<UserLocation> UserLocation { get; set; }
        public ICollection<UserMessage> UserMessageFkRecieverUser { get; set; }
        public ICollection<UserMessage> UserMessageFkSenderUser { get; set; }
    }
}