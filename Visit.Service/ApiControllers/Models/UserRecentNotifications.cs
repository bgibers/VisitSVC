using System;
using System.Collections.Generic;
using Visit.Service.Models;
using Visit.Service.Models.Responses;

namespace Visit.Service.ApiControllers.Models
{
    public class UserRecentNotifications
    {
        /// <summary>
        /// Recent comments for post w datetime as key for easy ordering
        /// </summary>
        public Dictionary<DateTime, CommentForPost> CommentsForPost { get; set; }

        /// <summary>
        /// Recent likes for post w datetime as key for easy ordering
        /// </summary>
        public Dictionary<DateTime, LikeForPost> LikesForPost { get; set; }
    }
}