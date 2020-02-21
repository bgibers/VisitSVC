using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class PostTypes
    {
        public PostTypes()
        {
            Posts = new HashSet<Posts>();
        }

        public int PostTypeId { get; set; }
        public string PostType { get; set; }

        public virtual ICollection<Posts> Posts { get; set; }
    }
}
