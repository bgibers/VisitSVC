using System;
using System.Collections.Generic;

namespace VisitSVC
{
    public class UserLocations
    {
        public int UserLocationId { get; set; }
        public int? FkLocationId { get; set; }
        public int? FkUserId { get; set; }
        public string Status { get; set; }
        public string Venue { get; set; }
        public string Tags { get; set; }

        public virtual Location FkLocation { get; set; }
        public virtual User FkUser { get; set; }
    }
}
