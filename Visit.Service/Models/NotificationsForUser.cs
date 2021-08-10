using System;
using Visit.Service.Models.Responses;

namespace Visit.Service.Models
{
    public class NotificationsForUser
    {
        public int FkPostId { get; set; }
        public string Comment { get; set; }
        public DateTime Date { get; set; }
        
        // The user who liked or commented on a post
        public SlimUserResponse UserWhoPerformedAction { get; set; }
    }
}