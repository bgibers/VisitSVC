namespace Visit.Service.Models.Responses
{
    public class LikeForPost
    {
        public int LikeId { get; set; }
        public int FkPostId { get; set; }
        public SlimUserResponse User { get; set; }
    }
}