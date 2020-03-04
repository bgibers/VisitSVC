namespace Visit.DataAccess.Models
{
    public partial class PostUserLocation
    {
        public int PostUserLocationId { get; set; }
        public int? FkPostId { get; set; }
        public int? FkLocationId { get; set; }

        public virtual UserLocation FkLocation { get; set; }
        public virtual Post FkPost { get; set; }
    }
}
