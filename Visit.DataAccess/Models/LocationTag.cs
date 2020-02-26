using System.ComponentModel.DataAnnotations.Schema;

namespace Visit.DataAccess.Models
{
    public partial class LocationTag
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LocationTagId { get; set; }
        public int? FkUserLocationId { get; set; }
        public int? FkTagId { get; set; }

        public virtual Tag FkTag { get; set; }
        public virtual UserLocation FkUserLocation { get; set; }
    }
}
