using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Visit.DataAccess.Models;
using Visit.Service.ApiControllers.Models;
using Visit.Service.ApiControllers.Models.Enums;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Config;

namespace Visit.Service.BusinessLogic
{
    public class AccountsBusinessLogic : IAccountsBusinessLogic
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;
        private readonly IOptions<CognitoConfig> _config;
        private readonly ILogger<AccountsBusinessLogic> _logger;

        public AccountsBusinessLogic(SignInManager<CognitoUser> signInManager, CognitoUserManager<CognitoUser> userManager, 
            CognitoUserPool pool, IOptions<CognitoConfig> config)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _pool = pool;
            _config = config;
        }

        public async Task<User> RegisterUser(RegisterModelApi model)
        {
            var user = _pool.GetUser(model.Email);
            if (!string.IsNullOrEmpty(user.Status))
            {
                return null;
            }
            
            await _userManager.CreateAsync(user, model.Password);
            var newUser = _pool.GetUser(model.Email);
            
            return new User();
        }

        public async Task<CodeConfirmResult> ConfirmRegister(CodeConfirmApi model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ChangePassword(ChangePasswordApi model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<bool> ForgotPassword(ResetPasswordRequestApi model)
        {
            throw new System.NotImplementedException();
        }

        public async Task<CodeConfirmResult> ConfirmPasswordReset(ResetPasswordApi model)
        {
            throw new System.NotImplementedException();
        }
    }
}