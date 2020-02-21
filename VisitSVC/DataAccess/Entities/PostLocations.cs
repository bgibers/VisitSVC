using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class PostLocations
    {
        public PostLocations()
        {
            UserTags = new HashSet<UserTags>();
        }

        public int PostLocationId { get; set; }
        public int? FkPostId { get; set; }
        public int? FkLocationId { get; set; }
        public string Status { get; set; }

        public virtual Location FkLocation { get; set; }
        public virtual Posts FkPost { get; set; }
        public virtual ICollection<UserTags> UserTags { get; set; }
    }
}
