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

        public int UserLocationId { get; set; }
        public int? FkLocationId { get; set; }
        public string FkUserId { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Venue { get; set; }
        public sbyte? CheckedOff { get; set; }

        [NotMapped]
        public bool IsCheckedOff
        {
            get => CheckedOff > 0;
            set { this.CheckedOff = (sbyte)(value ? 1 : 0);  }
        }
        
        public virtual Location FkLocation { get; set; }
        public virtual User FkUser { get; set; }
        public virtual ICollection<LocationTag> LocationTag { get; set; }
        public virtual ICollection<PostUserLocation> PostUserLocation { get; set; }
    }
}
