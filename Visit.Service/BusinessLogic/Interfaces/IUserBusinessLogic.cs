using System.Collections.Generic;
using System.Threading.Tasks;
using Visit.Service.Models;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IUserBusinessLogic
    {
        Task<UserResponse> GetUserById(string id);

        Task<SlimUserResponse> GetLoggedInUser(string claim);
        
        Task<SlimUserResponse> GetUserByEmail(string email);

        /// <summary>
        /// For the instances when we only want the users name and image
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SlimUserResponse> GetSlimUser(string id);
        
        /// <summary>
        /// Return the 25 latest notifications for a user
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        Task<List<NotificationsForUser>> GetUserRecentNotifications(string claim);
        
        /// <summary>
        /// For searching 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<SlimUserResponse> FindUserBySearchCriteria(string query);
    }
}