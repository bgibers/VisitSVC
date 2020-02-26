using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VisitSVC.DataAccess.Models;
using VisitSVC.Services;

namespace VisitSVC.Controllers
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
        public Task<List<User>> GetAll()
        {
            return _dataService.GetUsers();
        }

        [HttpPost("post")]
        public async Task<ActionResult<User>> PostTest()
        {
            return new ObjectResult(await _dataService.CreateUsers());
        }
    }
}