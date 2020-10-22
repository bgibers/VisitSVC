using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;

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
        [HttpGet("posts/{page}")]
        [ProducesResponseType(200)]
        public async Task<PaginatedList<Post>> GetPostsForPage(int page)
        { 
            return await _postService.GetPostsByPage(page);
        }
    }
}