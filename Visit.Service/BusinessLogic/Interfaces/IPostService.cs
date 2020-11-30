using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Visit.DataAccess.Models;
using Visit.Service.Models;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByUser(string userId);

        /// <summary>
        /// Get 50 posts by page number. Sorting by date in desc order 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<PaginatedList<PostApi>> GetPostsByPage(int? pageNumber);

        /// <summary>
        /// Create a new post
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="postRequest"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        Task<NewPostResponse> CreatePost(Claim claim, CreatePostRequest postRequest);

        /// <summary>
        /// Allows a user to like a post
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="postId"></param>
        /// <returns></returns>
        Task<bool> LikePost(Claim claim, string postId);

        /// <summary>
        /// Allows a user to comment on a post
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="postId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        Task<bool> CommentOnPost(Claim claim, string postId, string comment);

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