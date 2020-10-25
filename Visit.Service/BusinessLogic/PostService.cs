using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Extenstions;
using Visit.Service.Models.Responses;

namespace Visit.Service.BusinessLogic
{
    public class PostService : IPostService
    {
        
        private readonly VisitContext _context;
        private readonly IUserBusinessLogic _userBusinessLogic;
        private readonly IMapper _mapper;
        
        public PostService(VisitContext context, IUserBusinessLogic userBusinessLogic, IMapper mapper)
        {
            _context = context;
            _userBusinessLogic = userBusinessLogic;
            _mapper = mapper;
        }
        
        public async Task<List<Post>> GetPostsByUser(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PaginatedList<Post>> GetPostsByPage(int? pageNumber)
        {
            int pageSize = 50;
            var postList = _context.Post.OrderByDynamic("PostTime", "OrderByDescending")
                .Include(p => p.FkUser)
                .Include(p => p.FkPostType)
                .Include(p => p.PostUserLocation)
                .ThenInclude(p => p.FkLocation);

            foreach (var post in postList)
            {
                // todo properly return userResponse here instead of scrubbing the data
                var avi = _userBusinessLogic.GetUserAvi(post.FkUser.Avi);
                var temp = _mapper.Map<UserResponse>(post.FkUser);
                temp.Avi = avi;
                post.FkUser = _mapper.Map<User>(temp);
            }
            
            return await PaginatedList<Post>.CreateAsync(postList.AsNoTracking(), pageNumber ?? 1, pageSize);
        }
    }
}