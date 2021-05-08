using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using Visit.Service.BusinessLogic.Interfaces;

namespace Visit.Service.BusinessLogic
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        public FirebaseService(FirebaseMessaging firebaseMessaging)
        {
            _firebaseMessaging = firebaseMessaging;
        }

        public Task<string> SendPushNotification(Message message)
        {
            return _firebaseMessaging.SendAsync(message);
        }
    }
}