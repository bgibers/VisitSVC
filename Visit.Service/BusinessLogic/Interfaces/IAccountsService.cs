using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Visit.DataAccess.Auth;
using Visit.DataAccess.Models;
using Visit.Service.Models;
using Visit.Service.Models.Enums;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IAccountsService
    {
        Task<CreateUserResponse> RegisterUser(RegisterRequest model);
        Task<CodeConfirmResult> ConfirmRegister(CodeConfirmRequest model);
        Task<bool> ChangePassword(ChangePasswordRequest model);
        Task<bool> ForgotPassword(ResetPasswordRequest model);
        Task<CodeConfirmResult> ConfirmPasswordReset(SetNewPasswordWithCodeRequest model);
        Task<JwtToken> LoginUser(LoginApiRequest credentials);
        Task<UploadImageResponse> UpdateProfileImage(Claim user, IFormFile image);

        Task<bool> EmailAlreadyTaken(string email);
    }
}