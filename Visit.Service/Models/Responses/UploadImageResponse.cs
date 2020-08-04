using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Visit.Service.Models.Responses
{
    public class UploadImageResponse
    {
        public bool Success { get; }
        
        public ImageErrors Errors { get; }
        public UploadImageResponse(bool success = false, ImageErrors errors = null)
        {
            Success = success;
            Errors = errors;
        }
    }

    public class ImageErrors
    {
        public IEnumerable<IdentityError> IdentityErrors {
            get;
            set;
        }
        public string UploadError { get; set; }
    }
}