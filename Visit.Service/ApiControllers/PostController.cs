using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.GetPostsByPage(headerValue.Parameter, page);
            }

            return Unauthorized();
        }

        /// <summary>
        /// Get posts by page. Each page is 50 results
        /// </summary>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{page}/{filter}/{userId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedList<PostApi>>> GetPostsForPageWithFilterByUserId(int page, string filter = "", string userId = "")
        { 
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.GetPostsByPage(headerValue.Parameter, page, filter, userId);
            }

            return Unauthorized();
        }
        
        /// <summary>
        /// Get posts by page. Each page is 50 results
        /// </summary>
        /// <param name="page"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{page}/user/{userId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedList<PostApi>>> GetPostsForPageByUserId(int page, string userId = "")
        { 
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.GetPostsByPage(headerValue.Parameter, page, "", userId);
            }

            return Unauthorized();
        }
        
        /// <summary>
        /// Get posts by page. Each page is 50 results
        /// </summary>
        /// <param name="page"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [HttpGet("{page}/filter/{filter}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<PaginatedList<PostApi>>> GetPostsForPageWithFilter(int page, string filter = "")
        { 
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.GetPostsByPage(headerValue.Parameter, page, filter);
            }

            return Unauthorized();
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        [HttpPost("new")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<NewPostResponse>> AddNewPost([FromForm] CreatePostRequest post)
        { 
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.CreatePost(headerValue.Parameter, post);
            }

            return Unauthorized();
            
            
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
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.LikePost(headerValue.Parameter, postId);
            }
            return Unauthorized();
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
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _postService.CommentOnPost(headerValue.Parameter, postId, comment.Comment);
            }
            return Unauthorized();
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