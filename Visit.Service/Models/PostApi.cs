using System;
using Visit.DataAccess.Models;
using Visit.Service.Models.Responses;

namespace Visit.Service.Models
{
    public class PostApi
    {
        public int PostId { get; set; }
        public int? FkPostTypeId { get; set; }
        public string FkUserId { get; set; }
        public string PostContentLink { get; set; }
        public string PostCaption { get; set; }
        public DateTime? PostTime { get; set; }
        public int? ReviewRating { get; set; }
        public PostType FkPostType { get; set; }
        public UserResponse FkUser { get; set; }
        public Location Location { get; set; }
        
        public int CommentCount { get; set; }
        
        public int LikeCount { get; set; }
    }
}