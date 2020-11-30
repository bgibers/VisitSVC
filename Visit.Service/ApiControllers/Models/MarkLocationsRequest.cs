using System.Collections.Generic;

namespace Visit.Service.Models.Requests
{
    public class MarkLocationsRequest
    {
        /// <summary>
        /// This dictionary is a mapping of the requests that are coming in. <locationCode,Status>
        /// </summary>
        public Dictionary<string, string> Locations { get; set; }
    }
}