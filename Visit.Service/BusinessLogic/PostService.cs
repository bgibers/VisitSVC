using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;
using Visit.Service.Models;
using Visit.Service.Models.Extenstions;

namespace Visit.Service.BusinessLogic
{
    public class PostService : IPostService
    {
        private readonly VisitContext _context;

        public PostService(VisitContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPostsByUser(string userId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PaginatedList<Post>> GetList(int? pageNumber, string sortField, string sortOrder)
        {
            int pageSize = 50;
            var postList = _context.Post.OrderByDynamic(sortField, sortOrder.ToUpper());
            return await PaginatedList<Post>.CreateAsync(postList.AsNoTracking(), pageNumber ?? 1, pageSize);
        }
    }
}