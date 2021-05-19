using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using Visit.Service.BusinessLogic.Interfaces;

namespace Visit.Service.BusinessLogic
{
    public class FirebaseService : IFirebaseService
    {
        private readonly FirebaseMessaging _firebaseMessaging;

        private readonly FirebaseAuth _firebaseAuth;

        public FirebaseService(FirebaseMessaging firebaseMessaging, FirebaseAuth firebaseAuth)
        {
            _firebaseMessaging = firebaseMessaging;
            _firebaseAuth = firebaseAuth;
        }

        public Task<string> SendPushNotification(Message message)
        {
            return _firebaseMessaging.SendAsync(message);
        }

        public async Task<UserRecord> CreateUser(string email, string password)
        {
            return await _firebaseAuth.CreateUserAsync(new UserRecordArgs()
            {
                Email = email,
                Password = password
            });
        }
        
        public async Task<UserRecord> GetUserByEmail(string email)
        {
            return await _firebaseAuth.GetUserByEmailAsync(email);
        }

        public async Task<FirebaseToken> GetUserFromToken(string uuid)
        {
            return await _firebaseAuth.VerifyIdTokenAsync(uuid);
        }
    }
}