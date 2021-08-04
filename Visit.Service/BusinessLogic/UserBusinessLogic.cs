using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Extenstions;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly ILogger<UserBusinessLogic> _logger;
        private readonly IMapper _mapper;
        private readonly VisitContext _visitContext;
        private readonly IFirebaseService _firebaseService;

        public UserBusinessLogic(ILogger<UserBusinessLogic> logger, IMapper mapper, 
            VisitContext visitContext, IFirebaseService firebaseService)
        {
            _logger = logger;
            _mapper = mapper;
            _visitContext = visitContext;
            _firebaseService = firebaseService;
        }
        
        public async Task<UserResponse> GetUserById(string id)
        {
            
            var user = await _visitContext.User
//                .Include(u => u.FkBirthLocation)
//                .Include(u => u.FkResidenceLocation)
                .Include(u => u.UserFollowingFkFollowUser)
                .Include(u => u.UserFollowingFkMainUser)
                .Include(u => u.UserLocation)
                    .ThenInclude(l => l.FkLocation)
                .SingleOrDefaultAsync(u => u.Id == id);
            
            // take all sensitve data out
            var userScrubbed = _mapper.Map<UserResponse>(user);
            userScrubbed.Avi = user.Avi ?? "";
            userScrubbed.FollowerCount = user.UserFollowingFkFollowUser.Count;
            userScrubbed.FollowingCount = user.UserFollowingFkMainUser.Count;
            userScrubbed.UserId = id;

            return userScrubbed;
        }

        public async Task<SlimUserResponse> GetLoggedInUser(string claim)
        {
            var userId = (await _firebaseService.GetUserFromToken(claim)).Uid;
            var user = await _visitContext.User.SingleAsync(u => u.Id == userId);
            
            return _mapper.Map<SlimUserResponse>(user);
        }


        public async Task<SlimUserResponse> GetUserByEmail(string email)
        {
            var userId = (await _firebaseService.GetUserByEmail(email)).Uid;
            var user = await _visitContext.User.SingleAsync(u => u.Id == userId);
            
            return _mapper.Map<SlimUserResponse>(user);
        }

        public List<SlimUserResponse> FindUserBySearchCriteria(string query)
        {
            var queries = query.Split(' ');
            var users = _visitContext.User
                .Where(u => u.Firstname.ToLower().Contains(queries[0].ToLower()) 
                            || u.Lastname.ToLower().Contains(queries[0].ToLower())).Take(20);

            if (queries.Length > 1)
            {
                users = users.Where(u => u.Lastname.ToLower().Contains(queries[1].ToLower()));
            }
            
            return _mapper.Map<List<SlimUserResponse>>(users.ToList());
        }

        public async Task<SlimUserResponse> GetSlimUser(string id)
        {
            var user = await _visitContext.User.SingleAsync(u => u.Id == id);
            
            return _mapper.Map<SlimUserResponse>(user);
        }
        
        public async Task<List<NotificationsForUser>> GetUserRecentNotifications(string claim)
        {
            var user = await _firebaseService.GetUserFromToken(claim);
            var notificationsToReturn = new List<NotificationsForUser>();
            
            var userNotifications = await _visitContext.UserNotification.Where(u => u.FkUserId == user.Uid)
                .OrderByDynamic("DatetimeOfNot", "OrderByDescending")
                .Include(u => u.FkUser)
                .Include(u => u.PostComment)
                .ToListAsync();

            foreach (var notification in userNotifications.Take(25))
            {
                notificationsToReturn.Add(new NotificationsForUser()
                {
                    FkPostId = notification.FkPostId,
                    Comment = notification.PostComment == null ? "" : notification.PostComment.CommentText,
                    UserWhoPerformedAction = _mapper.Map<User, SlimUserResponse>(notification.FkUser),
                    Date = notification.DatetimeOfNot
                });
            }

            return notificationsToReturn;
        }
    }
}