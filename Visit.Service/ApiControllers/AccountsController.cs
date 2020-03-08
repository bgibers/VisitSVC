using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Visit.DataAccess.Models;
using Visit.Service.ApiControllers.Models;
using Visit.Service.BusinessLogic.Interfaces;

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
        [ProducesResponseType(typeof(User), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterModelApi modelApi)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _accountsService.RegisterUser(modelApi);

            if (user == null)
            {
                ModelState.AddModelError("UserExists", "Username or email already registered");
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

//        [HttpPost("confirm")]
//        [ProducesResponseType(typeof(CodeConfirmResult),200)]
//        public async Task<IActionResult> ConfirmRegister([FromBody]CodeConfirmApi api)
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
//        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordApi model)
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
//        public async Task<IActionResult> ForgotPassword([FromBody]ResetPasswordRequestApi model)
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
//        public async Task<IActionResult> ConfirmResetPassword([FromBody]ResetPasswordApi model)
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