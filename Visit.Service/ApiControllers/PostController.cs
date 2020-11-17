using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<PaginatedList<Post>> GetPostsForPage(int page)
        { 
            return await _postService.GetPostsByPage(page);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="post"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        [HttpPost("new")]
        [ProducesResponseType(200)]
        public async Task<NewPostResponse> AddNewPost([FromForm] CreatePostRequest post)
        { 
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            return await _postService.CreatePost(user, post);
        }
    }
}