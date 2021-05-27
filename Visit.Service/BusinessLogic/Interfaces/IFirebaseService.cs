using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> SendPushNotification(Message message);
        
        Task<string> CustomJwt(string uuid);

        Task<string> VerifyUser(string email);

        Task<string> ResetPassword(string email);
        
        Task<UserRecord> CreateUser(string email, string password);
        
        Task<UserRecord> GetUserByEmail(string email);

        Task<bool> CheckIfUserIsVerified(string uuid);
        
        Task<FirebaseToken> GetUserFromToken(string uuid);
    }
}