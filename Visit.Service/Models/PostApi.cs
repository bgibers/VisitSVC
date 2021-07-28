using System;
using Visit.DataAccess.Models;
using Visit.Service.Models.Responses;

namespace Visit.Service.Models
{
    public class PostApi
    {
        /// <summary>
        /// The primary ID
        /// </summary>
        public int PostId { get; set; }
        
        /// <summary>
        /// Post type FK 
        /// </summary>
        public int? FkPostTypeId { get; set; }
      
        /// <summary>
        /// User fk of post
        /// </summary>
        public string FkUserId { get; set; }
        
        /// <summary>
        /// Link to image
        /// </summary>
        public string PostContentLink { get; set; }
       
        /// <summary>
        /// Caption of post
        /// </summary>
        public string PostCaption { get; set; }
      
        /// <summary>
        /// Time of post, in UTC
        /// </summary>
        public DateTime? PostTime { get; set; }
       
        /// <summary>
        /// Future use, rating of trip etc
        /// </summary>
        public int? ReviewRating { get; set; }
       
        /// <summary>
        /// Post type object
        /// </summary>
        public PostType FkPostType { get; set; }
    
        /// <summary>
        /// User that posted
        /// </summary>
        public UserResponse FkUser { get; set; }
       
        /// <summary>
        /// Location posted about
        /// </summary>
        public Location Location { get; set; }
        
        /// <summary>
        /// Whether or not the current user has liked the post
        /// </summary>
        public bool LikedByCurrentUser { get; set; }
        
        /// <summary>
        /// Number of comments
        /// </summary>
        public int CommentCount { get; set; }
        
        /// <summary>
        /// Number of likes
        /// </summary>
        public int LikeCount { get; set; }
    }
}