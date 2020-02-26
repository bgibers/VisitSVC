using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visit.DataAccess.Models
{
    public partial class UserLocation
    {
        public UserLocation()
        {
            LocationTag = new HashSet<LocationTag>();
            PostUserLocation = new HashSet<PostUserLocation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserLocationId { get; set; }
        public int? FkLocationId { get; set; }
        public int? FkUserId { get; set; }
        public string Status { get; set; }
        public string Venue { get; set; }

        public virtual Location FkLocation { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<LocationTag> LocationTag { get; set; }
        public virtual ICollection<PostUserLocation> PostUserLocation { get; set; }
    }
}
