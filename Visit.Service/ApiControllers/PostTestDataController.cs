using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Visit.DataAccess.Models;
using Visit.Service.BusinessLogic;

namespace Visit.Service.ApiControllers
{
    [Route("api/TestData")]
    [ApiController]
    [Authorize(Policy = "VisitUser")]
    public class PostTestDataController
    {
        private readonly PostTestDataService _dataService;

        public PostTestDataController(PostTestDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<User>), 200)]
        public Task<List<User>> GetAll()
        {
            return _dataService.GetUsers();
        }

        [HttpPost("post")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> PostTest()
        {
            return new ObjectResult(await _dataService.CreateUsers());
        }
    }
}