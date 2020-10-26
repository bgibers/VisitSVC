using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly IMapper _mapper;
        
        public PostService(VisitContext context, IMapper mapper)
        {
            _context = context;
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
                var temp = _mapper.Map<UserResponse>(post.FkUser);
                post.FkUser = _mapper.Map<User>(temp);
            }

            return await PaginatedList<Post>.CreateAsync(postList.AsNoTracking(), pageNumber ?? 1, pageSize);
        }
    }
}