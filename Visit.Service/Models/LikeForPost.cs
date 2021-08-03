using System;

namespace Visit.Service.Models.Responses
{
    public class LikeForPost
    {
        public int LikeId { get; set; }
        public int FkPostId { get; set; }
        
        public DateTime TimeOfLike { get; set; }

        public SlimUserResponse User { get; set; }
    }
}