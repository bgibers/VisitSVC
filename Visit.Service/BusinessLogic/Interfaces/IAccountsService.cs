using System.Threading.Tasks;
using Visit.DataAccess.Models;
using Visit.Service.Models;
using Visit.Service.Models.Enums;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IAccountsService
    {
        Task<UserApi> RegisterUser(RegisterModelApi model);
        Task<CodeConfirmResult> ConfirmRegister(CodeConfirmApi model);
        Task<bool> ChangePassword(ChangePasswordApi model);
        Task<bool> ForgotPassword(ResetPasswordRequestApi model);
        Task<CodeConfirmResult> ConfirmPasswordReset(ResetPasswordApi model);
    }
}