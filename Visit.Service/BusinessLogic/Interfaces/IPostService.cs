using System.Collections.Generic;
using System.Threading.Tasks;
using Visit.DataAccess.Models;
using Visit.Service.Models;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsByUser(string userId);
        
        Task<PaginatedList<Post>> GetList(int? pageNumber, string sortField, string sortOrder);

    }
}