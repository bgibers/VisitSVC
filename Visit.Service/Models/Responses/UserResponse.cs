using System;
using System.Collections.Generic;
using Visit.DataAccess.Models;

namespace Visit.Service.Models.Responses
{
    public class UserResponse
    {
        public string UserId { get; set; }
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
        /// All posts by the user
        /// </summary>
        public virtual ICollection<Post> Posts { get; set; }
        
        /// <summary>
        /// The number of users the user follows
        /// </summary>
        public int FollowingCount { get; set; }
        
        /// <summary>
        /// Count of the users followers
        /// </summary>
        public int FollowerCount { get; set; }
        
        /// <summary>
        /// All locations that this user has marked 
        /// </summary>
        public virtual ICollection<UserLocation> UserLocations { get; set; }
    }
}