using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class Location
    {
        public Location()
        {
            PostLocations = new HashSet<PostLocations>();
            UserFkBirthLocation = new HashSet<User>();
            UserFkResidenceLocation = new HashSet<User>();
            UserLocations = new HashSet<UserLocations>();
        }

        public int LocationId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string LocationType { get; set; }
        public string LocationCountry { get; set; }

        public virtual ICollection<PostLocations> PostLocations { get; set; }
        public virtual ICollection<User> UserFkBirthLocation { get; set; }
        public virtual ICollection<User> UserFkResidenceLocation { get; set; }
        public virtual ICollection<UserLocations> UserLocations { get; set; }
    }
}
