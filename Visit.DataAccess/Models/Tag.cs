﻿using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public partial class Tag
    {
        public Tag()
        {
            LocationTag = new HashSet<LocationTag>();
            PostTag = new HashSet<PostTag>();
        }

        public int TagId { get; set; }
        public string Tag1 { get; set; }

        public virtual ICollection<LocationTag> LocationTag { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
    }
}
