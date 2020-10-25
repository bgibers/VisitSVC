using System.Collections.Generic;
using System.Threading.Tasks;
using Visit.DataAccess.Models;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IUserBusinessLogic
    {
        Task<UserResponse> GetUserById(string id);
        
        Task<UserResponse> GetUserByEmail(string email);

        /// <summary>
        /// For the instances when we only want the users name and image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SlimUserResponse> GetSlimUser(string id);

        /// <summary>
        /// Get the avi location from blob storage
        /// </summary>
        /// <param name="aviLocation"></param>
        /// <returns></returns>
        string GetUserAvi(string aviLocation);

        /// <summary>
        /// For searching 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<SlimUserResponse> FindUserBySearchCriteria(string query);
    }
}