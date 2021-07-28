using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models.Responses;

namespace Visit.Service.ApiControllers
{
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly VisitContext _context;
        private readonly IUserBusinessLogic _userBusinessLogic;

        public UserController(VisitContext context, IUserBusinessLogic userBusinessLogic)
        {
            _context = context;
            _userBusinessLogic = userBusinessLogic;
        }

        [Authorize]
        [HttpGet("self")]
        public async Task<ActionResult<SlimUserResponse>> GetAuthUser()
        {

            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _userBusinessLogic.GetLoggedInUser(headerValue.Parameter);
            }
            
            return Unauthorized();

        }
        
        
        /// <summary>
        /// Get all users registered
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.User.ToListAsync();
        }

        /// <summary>
        /// Get a user by their ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserResponse>> GetUser(string id)
        {
            return await _userBusinessLogic.GetUserById(id);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("search/{query}")]
        [Authorize]
        public List<SlimUserResponse> Search(string query)
        {
            return _userBusinessLogic.FindUserBySearchCriteria(query);
        }
    }
}