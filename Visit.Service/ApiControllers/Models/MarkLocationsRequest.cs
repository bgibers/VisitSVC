using System.Collections.Generic;

namespace Visit.Service.ApiControllers.Models
{
    public class MarkLocationsRequest
    {
        /// <summary>
        /// This dictionary is a mapping of the requests that are coming in. <locationCode,Status>
        /// </summary>
        public Dictionary<string, string> Locations { get; set; }

        /// <summary>
        /// If coming from registration we won't add new posts 
        /// </summary>
        public bool Registration { get; set; } = false;
    }
}