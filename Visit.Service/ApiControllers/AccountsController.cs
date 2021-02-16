using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visit.DataAccess.Auth;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models.Requests;

namespace Visit.Service.ApiControllers
{
    [EnableCors("CorsPolicy")]
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
        public async Task<ActionResult<JwtToken>> Register([FromBody] RegisterRequest request)
        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await _accountsService.RegisterUser(request);

            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return response.JwtToken;
        }
        
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="requestApi"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<JwtToken>> Login([FromBody] LoginApiRequest requestApi)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Todo create a response for logging in user
            return await _accountsService.LoginUser(requestApi);
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
        /// <returns></returns>
        [Authorize(Policy = "VisitUser")]
        [HttpPost("update/profile_image")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateProfileImage([FromForm]IFormFile image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var response = await _accountsService.UpdateProfileImage(user , image);
            
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return new OkResult();
        }
        
        /// <summary>
        /// Updates the logged in users info
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        [Authorize(Policy = "VisitUser")]
        [HttpPost("update")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateAccountInfo(UpdateUserInfoRequest update)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            return await _accountsService.UpdateAccountInfo(user , update);
        }
        
        /// <summary>
        /// Updates the status of world locations
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Policy = "VisitUser")]
        [HttpPost("update/locations")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateLocationStatus([FromBody] MarkLocationsRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            await _accountsService.ChangeLocationStatus(user, request);

            return true;
        }

//        [HttpPost("confirm")]
//        [ProducesResponseType(typeof(CodeConfirmResult),200)]
//        public async Task<IActionResult> ConfirmRegister([FromBody]CodeConfirmRequest api)
//        {
//
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//
//            var user = await _userManager.FindByEmailAsync(api.Email).ConfigureAwait(false);
//
//            if (user == null)
//            {
//                ModelState.AddModelError("NotFound", "Username or email not found");
//                return BadRequest(ModelState);
//            }
//
//            var result = await (_userManager as CognitoUserManager<CognitoUser>)
//                    .ConfirmSignUpAsync(user, api.Code, true).ConfigureAwait(false);
//
//            if (result.Succeeded) return Ok();
//
//            result.Errors.ToList().ForEach(u => ModelState.AddModelError(u.Code, u.Description));
//
//            return BadRequest(ModelState);
//
//
//        }
//
//        // https://github.com/aws/aws-aspnet-cognito-identity-provider/blob/master/docs/5-User%20Management%20-%20Change%20and%20reset%20passwords.md
//        [HttpPost("password/change")]
//        [ProducesResponseType(typeof(bool),200)]
//        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordRequest model)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//
//            var user = await _userManager.FindByEmailAsync(model.Email);
//
//            var result = await (_userManager as CognitoUserManager<CognitoUser>)
//                    .ChangePasswordAsync(user,model.OldPassword,model.NewPassword);
//
//            if (result.Succeeded) return Ok();
//
//            result.Errors.ToList().ForEach(u => ModelState.AddModelError(u.Code, u.Description));
//
//            return BadRequest(ModelState);
//        }
//
//        [HttpPost("password/forgot")]
//        [ProducesResponseType(typeof(bool),200)]
//        public async Task<IActionResult> ForgotPassword([FromBody]ResetPasswordRequest model)
//        {
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//            
//            var user = await _userManager.FindByEmailAsync(model.Email);
//
//            var result = await (_userManager as CognitoUserManager<CognitoUser>)
//                    .ResetPasswordAsync(user);
//
//           if (result.Succeeded) return Ok();
//
//            result.Errors.ToList().ForEach(u => ModelState.AddModelError(u.Code, u.Description));
//
//            return BadRequest(ModelState);
//        }
//
//
//        [HttpPost("password/confirmreset")]
//        [ProducesResponseType(typeof(CodeConfirmResult),200)]
//        public async Task<IActionResult> ConfirmResetPassword([FromBody]SetNewPasswordWithCodeRequest model)
//        {
//
//            if (!ModelState.IsValid) return BadRequest(ModelState);
//
//            var user = await _userManager.FindByEmailAsync(model.Email).ConfigureAwait(false);
//
//            if (user == null)
//            {
//                ModelState.AddModelError("NotFound", "Username or email not found");
//                return BadRequest(ModelState);
//            }
//
//            var result = await (_userManager as CognitoUserManager<CognitoUser>)
//                    .ResetPasswordAsync(user, model.Code, model.NewPassword);
//
//            if (result.Succeeded) return Ok();
//
//            result.Errors.ToList().ForEach(u => ModelState.AddModelError(u.Code, u.Description));
//
//            return BadRequest(ModelState);
//        }
    }
}