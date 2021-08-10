using System.Collections.Generic;
using System.Threading.Tasks;
using Visit.Service.Models;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IPostService
    {
        /// <summary>
        /// Get 50 posts by page number. Sorting by date in desc order 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="pageNumber"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<PaginatedList<PostApi>> GetPostsByPage(string user, int? pageNumber, string filter = "",
            string userId = "");

        /// <summary>
        /// Get a post by its id
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<PostApi> GetPostById(string claim, int postId);

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="user"></param>
        /// <param name="postRequest"></param>
        /// <returns></returns>
        Task<NewPostResponse> CreatePost(string user, CreatePostRequest postRequest);

        /// <summary>
        /// Allows a user to like a post
        /// </summary>
        /// <param name="user"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<bool> LikePost(string user, string postId);

        /// <summary>
        /// Allows a user to comment on a post
        /// </summary>
        /// <param name="user"></param>
        /// <param name="postId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<bool> CommentOnPost(string user, string postId, string comment);

        /// <summary>
        /// Get all likes for post by postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        List<LikeForPost> GetLikesForPost(string postId);

        /// <summary>
        /// Gets all comments for a post by postId
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        List<CommentForPost> GetCommentsForPost(string postId);
    }
}