using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace VisitSVC.DataAccess.Models
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
