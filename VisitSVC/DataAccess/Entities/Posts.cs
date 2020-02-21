using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class Posts
    {
        public Posts()
        {
            PostComments = new HashSet<PostComments>();
            PostLocations = new HashSet<PostLocations>();
        }

        public int PostId { get; set; }
        public int? FkPostTypeId { get; set; }
        public string PostContentLink { get; set; }
        public string PostCaption { get; set; }
        public int? FkUserId { get; set; }
        public int? ReviewRating { get; set; }

        public virtual PostTypes FkPostType { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<PostComments> PostComments { get; set; }
        public virtual ICollection<PostLocations> PostLocations { get; set; }
    }
}
