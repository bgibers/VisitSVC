using System.Threading.Tasks;
using Visit.DataAccess.Models;
using Visit.Service.ApiControllers.Models;
using Visit.Service.ApiControllers.Models.Enums;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IAccountsService
    {
        Task<User> RegisterUser(RegisterModelApi model);
        Task<CodeConfirmResult> ConfirmRegister(CodeConfirmApi model);
        Task<bool> ChangePassword(ChangePasswordApi model);
        Task<bool> ForgotPassword(ResetPasswordRequestApi model);
        Task<CodeConfirmResult> ConfirmPasswordReset(ResetPasswordApi model);
    }
}