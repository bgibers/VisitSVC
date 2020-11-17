using Microsoft.AspNetCore.Http;

namespace Visit.Service.Models.Requests
{
    public class CreatePostRequest
    {
        public string Caption { get; set; }
        
        public string PostType { get; set; }
        
        public string LocationCode { get; set; }

        public IFormFile? Image { get; set; }
        
    }
}