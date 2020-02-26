using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class Post
    {
        public Post()
        {
            PostComment = new HashSet<PostComment>();
            PostTag = new HashSet<PostTag>();
            PostUserLocation = new HashSet<PostUserLocation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }
        public int? FkPostTypeId { get; set; }
        public int? FkUserId { get; set; }
        public string PostContentLink { get; set; }
        public string PostCaption { get; set; }
        public int? ReviewRating { get; set; }

        public virtual PostType FkPostType { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<PostComment> PostComment { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
        public virtual ICollection<PostUserLocation> PostUserLocation { get; set; }
    }
}
