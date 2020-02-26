using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
{
    public partial class Tag
    {
        public Tag()
        {
            LocationTag = new HashSet<LocationTag>();
            PostTag = new HashSet<PostTag>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        public string Tag1 { get; set; }

        public virtual ICollection<LocationTag> LocationTag { get; set; }
        public virtual ICollection<PostTag> PostTag { get; set; }
    }
}
