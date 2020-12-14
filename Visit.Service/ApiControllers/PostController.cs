using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.ApiControllers
{
    [Route("posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }
        
        /// <summary>
        /// Get posts by page. Each page is 50 results
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet("{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedList<PostApi>>> GetPostsForPage(int page)
        { 
            var user = User.FindFirst(ClaimTypes.NameIdentifier);

            if (user == null)
            {
                return Unauthorized();
            }
            
            return await _postService.GetPostsByPage(user, page);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("new")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<NewPostResponse>> AddNewPost([FromForm] CreatePostRequest post)
        { 
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await _postService.CreatePost(user, post);
        }

        #region Likes and comments

        /// <summary>
        /// LIke a post by its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpPost("like/{postId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> LikePost(string postId)
        { 
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await _postService.LikePost(user, postId);
        }
        
        /// <summary>
        /// Get likes for a post by its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("likes/get/{postId}")]
        [ProducesResponseType(200)]
        public ActionResult<List<LikeForPost>> GetAllLikesForPost(string postId)
        { 
            return _postService.GetLikesForPost(postId);
        }

        /// <summary>
        /// Comment on a post by its Id
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost("comment/{postId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> CommentOnPost(string postId, [FromBody] CommentApi comment)
        { 
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            
            if (user == null)
            {
                return Unauthorized();
            }
            
            return await _postService.CommentOnPost(user, postId, comment.Comment);
        }
        
        /// <summary>
        /// Get comments for a post by its ID
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        [HttpGet("comments/get/{postId}")]
        [ProducesResponseType(200)]
        public ActionResult<List<CommentForPost>> GetAllCommentsForPost(string postId)
        { 
            return _postService.GetCommentsForPost(postId);
        }

        #endregion
    }
}