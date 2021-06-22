using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models.Requests;

namespace Visit.Service.ApiControllers
{
    [Route("account")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountsService _accountsService;

        public AccountsController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }

        /// <summary>
        /// Basic registration 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<string>> Register([FromBody] RegisterRequest request)
        {
            return await _accountsService.RegisterUser(request);
        }
        
        /// <summary>
        /// Checks if the email has already registered
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("email_taken")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> EmailTaken(string email)
        { 
            return await _accountsService.EmailAlreadyTaken(email);
        }
        
        /// <summary>
        /// Updates the logged in users profile img
        /// </summary>
        /// <param name="image"></param>
        /// <param name="claim"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("update/profile_image")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateProfileImage([FromForm]IFormFile image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                var response = await _accountsService.UpdateProfileImage(headerValue.Parameter , image);
            
                if (!response.Success)
                {
                    return BadRequest(response.Errors);
                }
            }
            else
            {
                return Unauthorized();
            }
            
            return new OkResult();
        }

        [Authorize]
        [HttpPost("update/fcm/{deviceId}")]
        public async Task<ActionResult<bool>> UpdateUserFcm(string deviceId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _accountsService.UpdateUserFcm(headerValue.Parameter , deviceId);
            }

            return Unauthorized();
        }
        

        /// <summary>
        /// Updates the logged in users info
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("update")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateAccountInfo(UpdateUserInfoRequest update)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return await _accountsService.UpdateAccountInfo(headerValue.Parameter , update);
            }

            return Unauthorized();

        }

        /// <summary>
        /// Updates the status of world locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("update/locations")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateLocationStatus([FromBody] MarkLocationsRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                await _accountsService.ChangeLocationStatus(headerValue.Parameter, request);
            }
            else
            {
                return Unauthorized();
            }
            
            return true;
        }
    }
}