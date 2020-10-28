using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Storage.Blobs;
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

        public Task<UserResponse> GetUserByEmail(string email)
        {
            throw new System.NotImplementedException();
        }

        public List<SlimUserResponse> FindUserBySearchCriteria(string query)
        {
            var queries = query.Split(' ');
            var users = _userManager.Users
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
            var user = await _userManager.FindByIdAsync(id);
            
            return _mapper.Map<SlimUserResponse>(user);
        }
    }
}