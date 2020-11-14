using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Visit.Service.BusinessLogic.Interfaces;

namespace Visit.Service.ApiControllers
{
    [Route("devops")]
    [ApiController]
    public class DevopsController : Controller
    {
        private readonly IDevopsService _devopsService;

        public DevopsController(IDevopsService devopsService)
        {
            _devopsService = devopsService;
        }
        
        [HttpPost("post_types")]
        public async Task<IActionResult> AddPostTypes()
        {
            return Ok(await _devopsService.AddPostTypes());
        }
        
        [HttpPost("new/{type}")]
        public async Task<IActionResult> AddNewPostType(string type)
        {
            return Ok(await _devopsService.AddNewPostType(type));
        }
        
    }
}