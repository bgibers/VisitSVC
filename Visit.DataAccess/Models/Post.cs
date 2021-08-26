using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visit.DataAccess.Models
{
    public partial class Post
    {
        public Post()
        {
            Like = new HashSet<Like>();
            PostComment = new HashSet<PostComment>();
            PostTag = new HashSet<PostTag>();
            PostUserLocation = new HashSet<PostUserLocation>();
            UserNotification = new HashSet<UserNotification>();
        }

        public int PostId { get; set; }
        public int? FkPostTypeId { get; set; }
        public string FkUserId { get; set; }
        public string PostContentLink { get; set; }
        public string PostCaption { get; set; }
        public DateTime? PostTime { get; set; }
        public int? ReviewRating { get; set; }
        public sbyte? Deleted { get; set; }

        [NotMapped]
        public bool IsDeleted
        {
            get => Deleted > 0;
            set { this.Deleted = (sbyte)(value ? 1 : 0);  }
        }
        
        public virtual PostType FkPostType { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<Like> Like { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
        public virtual ICollection<PostUserLocation> PostUserLocation { get; set; }
        public virtual ICollection<UserNotification> UserNotification { get; set; }
    }
}
