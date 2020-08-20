using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models.Responses;

namespace Visit.Service.ApiControllers
{
    [Route("User")]
    [EnableCors("CorsPolicy")]
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

        /// <summary>
        /// Get all users registered
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.User.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get/{id}")]
        [Authorize(Policy = "VisitUser")]
        public async Task<ActionResult<UserResponse>> GetUser(string id)
        {
            return await _userBusinessLogic.GetUserById(id);
        }
    }
}