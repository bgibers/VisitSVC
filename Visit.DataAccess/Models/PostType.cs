using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public class PostType
    {
        public PostType()
        {
            Post = new HashSet<Post>();
        }

        public int PostTypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}