using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Visit.Service.ApiControllers.Models;
using Visit.Service.ApiControllers.Models.Enums;
using Visit.Service.BusinessLogic.Interfaces;
using User = Visit.DataAccess.Models.User;

namespace Visit.Service.BusinessLogic
{
    public class AccountsBusinessLogic : IAccountsBusinessLogic
    {
        private readonly ILogger<AccountsBusinessLogic> _logger;

        public AccountsBusinessLogic(ILogger<AccountsBusinessLogic> logger)
        {
            _logger = logger;
        }

        public async Task<User> RegisterUser(RegisterModelApi model)
        {
            throw new System.NotImplementedException();
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