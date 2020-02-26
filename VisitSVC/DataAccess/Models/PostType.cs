using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class PostType
    {
        public PostType()
        {
            Post = new HashSet<Post>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostTypeId { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Post> Post { get; set; }
    }
}
