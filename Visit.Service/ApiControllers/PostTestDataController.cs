using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Visit.DataAccess.Models;
using Visit.Service.Services;

namespace Visit.Service.ApiControllers
{
    [Route("api/TestData")]
    [ApiController]
    public class PostTestDataController
    {
        private readonly PostTestDataService _dataService;
        
        public PostTestDataController(PostTestDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("users")]
        [ProducesResponseType(typeof(List<User>),200)]
        public Task<List<User>> GetAll()
        {
            return _dataService.GetUsers();
        }

        [HttpPost("post")]
        [ProducesResponseType(typeof(int),200)]
        public async Task<ActionResult<int>> PostTest()
        {
            return new ObjectResult(await _dataService.CreateUsers());
        }
    }
}