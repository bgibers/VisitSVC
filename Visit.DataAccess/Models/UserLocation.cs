using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public partial class UserLocation
    {
        public UserLocation()
        {
            LocationTag = new HashSet<LocationTag>();
            PostUserLocation = new HashSet<PostUserLocation>();
        }

        public int UserLocationId { get; set; }
        public int? FkLocationId { get; set; }
        public string FkUserId { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }

        public virtual Location FkLocation { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<LocationTag> LocationTag { get; set; }
        public virtual ICollection<PostUserLocation> PostUserLocation { get; set; }
    }
}
