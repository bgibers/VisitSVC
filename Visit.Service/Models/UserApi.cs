using System;
using System.Collections.Generic;
using Visit.DataAccess.Models;

namespace Visit.Service.Models
{
    public class UserApi
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Avi { get; set; }
        public long? FacebookId { get; set; }
        public virtual Location BirthLocation { get; set; }
        public virtual Location ResidenceLocation { get; set; }
        
        /// <summary>
        /// Everything the user has liked
        /// </summary>
        public virtual ICollection<Like> Likes { get; set; }
        
        /// <summary>
        /// All posts by the user
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; }
        
        /// <summary>
        /// The users that this user follows
        /// </summary>
        public virtual ICollection<UserFollowing> Following { get; set; }
        
        /// <summary>
        /// All of this users followers
        /// </summary>
        public virtual ICollection<UserFollowing> Followers { get; set; }
        
        /// <summary>
        /// All locations that this user has marked 
        /// </summary>
        public virtual ICollection<UserLocation> UserLocations { get; set; }
    }
}