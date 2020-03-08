using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Visit.DataAccess.Models;
using Visit.Service.ApiControllers.Models;
using Visit.Service.ApiControllers.Models.Enums;
using Visit.Service.BusinessLogic.Interfaces;

namespace Visit.Service.BusinessLogic
{
    public class AccountsService : IAccountsService
    {
        private readonly ILogger<AccountsService> _logger;

        public AccountsService(ILogger<AccountsService> logger)
        {
            _logger = logger;
        }

        public async Task<User> RegisterUser(RegisterModelApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<CodeConfirmResult> ConfirmRegister(CodeConfirmApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ChangePassword(ChangePasswordApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ForgotPassword(ResetPasswordRequestApi model)
        {
            throw new NotImplementedException();
        }

        public async Task<CodeConfirmResult> ConfirmPasswordReset(ResetPasswordApi model)
        {
            throw new NotImplementedException();
        }
    }
}