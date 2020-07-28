using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Visit.DataAccess.Auth;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

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
        
        [HttpPost("login")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<JwtToken>> Login([FromBody] LoginApiRequest requestApi)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Todo create a response for logging in user
            return await _accountsService.LoginUser(requestApi);
        }
        
        [Authorize(Policy = "VisitUser")]
        [HttpPost("update/profile_image")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<bool>> UpdateProfileImage(IFormFile image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var response = await _accountsService.UpdateProfileImage(user ,image);
            
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            return new OkResult();
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