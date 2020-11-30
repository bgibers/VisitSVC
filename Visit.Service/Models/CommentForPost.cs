using System;
using Visit.Service.Models.Responses;

namespace Visit.Service.Models
{
    public class CommentForPost
    {
        public int CommentId { get; set; }
        public int FkPostId { get; set; }

        public string Comment { get; set; }
        public DateTimeOffset Date { get; set; }
        public SlimUserResponse User { get; set; }
    }
}