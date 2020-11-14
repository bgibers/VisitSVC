using System.Threading.Tasks;

namespace Visit.Service.BusinessLogic.Interfaces
{
    public interface IDevopsService
    {
        /// <summary>
        /// Add all hardcoded post types
        /// </summary>
        /// <returns></returns>
        Task<bool> AddPostTypes();

        /// <summary>
        /// Adds a new post type for on the fly development
        /// </summary>
        /// <returns></returns>
        Task<bool> AddNewPostType(string type);
    }
}