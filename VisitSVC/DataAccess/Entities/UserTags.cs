using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class UserTags
    {
        public int UserTags1 { get; set; }
        public int? FkUserId { get; set; }
        public bool? IsPostLocation { get; set; }
        public int? FkLocationPostId { get; set; }

        public virtual PostLocations FkLocationPost { get; set; }
        public virtual User FkUser { get; set; }
    }
}
