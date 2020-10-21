using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;

namespace Visit.Service.ApiControllers
{
    [Route("posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
    }
}