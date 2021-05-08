using System.Threading.Tasks;
using FirebaseAdmin.Messaging;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> SendPushNotification(Message message);
    }
}