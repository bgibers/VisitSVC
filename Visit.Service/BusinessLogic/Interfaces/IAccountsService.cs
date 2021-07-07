using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Visit.Service.ApiControllers.Models;
using Visit.Service.Models.Requests;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IAccountsService
    {
        Task<string> RegisterUser(RegisterRequest model);
        Task<UploadImageResponse> UpdateProfileImage(string claim, IFormFile image);
        Task<bool> UpdateAccountInfo(string claim, UpdateUserInfoRequest request);
        Task<int> ChangeLocationStatus(string claim, MarkLocationsRequest request);
        Task<bool> EmailAlreadyTaken(string email);
        /// <summary>
        /// Update the users FCM record. Used for sending push notifications
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<bool> UpdateUserFcm(string claim, string deviceId);
    }
}