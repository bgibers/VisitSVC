using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> SendPushNotification(Message message);

        Task<UserRecord> CreateUser(string email, string password);
        Task<UserRecord> GetUserByEmail(string email);
        Task<FirebaseToken> GetUserFromToken(string uuid);
    }
}