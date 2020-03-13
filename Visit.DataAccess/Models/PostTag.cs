namespace Visit.DataAccess.Models
{
    public class PostTag
    {
        public int PostTagId { get; set; }
        public int? FkPostId { get; set; }
        public int? FkTagId { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual Tag FkTag { get; set; }
    }
}