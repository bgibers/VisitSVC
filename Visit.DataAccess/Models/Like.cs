namespace Visit.DataAccess.Models
{
    public partial class Like
    {
        public int LikeId { get; set; }
        public int FkPostId { get; set; }
        public string FkUserId { get; set; }

        public virtual Post FkPost { get; set; }
        public virtual User FkUser { get; set; }
    }
}
