using System.Collections.Generic;

namespace Visit.DataAccess.Models
{
    public class Location
    {
        public Location()
        {
            UserFkBirthLocation = new HashSet<User>();
            UserFkResidenceLocation = new HashSet<User>();
            UserLocation = new HashSet<UserLocation>();
        }

        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public string LocationCountry { get; set; }

        public virtual ICollection<User> UserFkBirthLocation { get; set; }
        public virtual ICollection<User> UserFkResidenceLocation { get; set; }
        public virtual ICollection<UserLocation> UserLocation { get; set; }
    }
}