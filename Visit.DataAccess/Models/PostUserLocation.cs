using System.ComponentModel.DataAnnotations.Schema;

namespace Visit.DataAccess.Models
{
    public partial class PostUserLocation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostUserLocationId { get; set; }
        public int? FkPostId { get; set; }
        public int? FkLocationId { get; set; }

        public virtual UserLocation FkLocation { get; set; }
        public virtual Post FkPost { get; set; }
    }
}
