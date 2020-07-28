using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.BlobStorage;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic
{
    public class UserBusinessLogic : IUserBusinessLogic
    {
        private readonly ILogger<UserBusinessLogic> _logger;
        private readonly IMapper _mapper;
        private readonly VisitContext _visitContext;
        private readonly UserManager<User> _userManager;
        private readonly IBlobStorageBusinessLogic _blobStorage;

        public UserBusinessLogic(ILogger<UserBusinessLogic> logger, IMapper mapper, VisitContext visitContext, UserManager<User> userManager, IBlobStorageBusinessLogic blobStorage)
        {
            _logger = logger;
            _mapper = mapper;
            _visitContext = visitContext;
            _userManager = userManager;
            _blobStorage = blobStorage;
        }
        
        public async Task<UserResponse> GetUserById(string id)
        {
            
            var user = await _visitContext.User
                .Include(u => u.FkBirthLocation)
                .Include(u => u.FkResidenceLocation)
                .Include(u => u.Post)
                .Include(u => u.FkBirthLocation)
                .Include(u => u.UserFollowingFkFollowUser)
                .Include(u => u.UserFollowingFkMainUser)
                .Include(u => u.UserLocation)
                .SingleOrDefaultAsync(u => u.Id == id);
            
            // take all sensitve data out
            var userScrubbed = _mapper.Map<UserResponse>(user);
            userScrubbed.FollowerCount = user.UserFollowingFkFollowUser.Count;
            userScrubbed.FollowingCount = user.UserFollowingFkMainUser.Count;
            userScrubbed.UserId = id;

            return userScrubbed;
        }

        public async Task<UserResponse> GetUserByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SlimUserResponse> GetSlimUser(string id)
        {
            return _mapper.Map<SlimUserResponse>(await _userManager.FindByIdAsync(id));
        }
    }
}