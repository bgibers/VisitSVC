namespace Visit.Service.Models.Responses
{
    public class NewPostResponse
    {
        public bool Success { get; }
        
        public ImageErrors Errors { get; }
        public NewPostResponse(bool success = false, ImageErrors errors = null)
        {
            Success = success;
            Errors = errors;
        }
    }
}