using System.Collections.Generic;
using System.Threading.Tasks;
using Visit.DataAccess.Models;
using Visit.Service.Models;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByUser(string userId);
        
        /// <summary>
        /// Get 50 posts by page number. Sorting by date in desc order 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        Task<PaginatedList<Post>> GetPostsByPage(int? pageNumber);

    }
}