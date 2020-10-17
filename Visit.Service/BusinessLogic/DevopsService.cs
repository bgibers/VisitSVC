using System.Threading.Tasks;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic.Interfaces;

namespace Visit.Service.BusinessLogic
{
    public class DevopsService : IDevopsService
    {
        private readonly VisitContext _visitContext;

        public DevopsService(VisitContext visitContext)
        {
            _visitContext = visitContext;
        }

        public async Task<bool> AddPostTypes()
        {
            await _visitContext.PostType.AddAsync(new PostType() {Type = "toVisit"});
            await _visitContext.PostType.AddAsync(new PostType() {Type = "visited"});

            await _visitContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> AddNewPostType()
        {
            throw new System.NotImplementedException();
        }
    }
}