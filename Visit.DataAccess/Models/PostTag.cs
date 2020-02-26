using System.ComponentModel.DataAnnotations.Schema;

namespace Visit.DataAccess.Models
{
    public partial class PostTag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostTagId { get; set; }
        public int? FkPostId { get; set; }
        public int? FkTagId { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual Tag FkTag { get; set; }
    }
}
