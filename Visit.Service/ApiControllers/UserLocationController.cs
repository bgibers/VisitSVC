using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visit.DataAccess.EntityFramework;
using Visit.DataAccess.Models;

namespace Visit.Service.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLocationController : ControllerBase
    {
        private readonly VisitContext _context;

        public UserLocationController(VisitContext context)
        {
            _context = context;
        }

        // GET: api/UserLocation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserLocation>>> GetUserLocation()
        {
            return await _context.UserLocation.ToListAsync();
        }

        // GET: api/UserLocation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserLocation>> GetUserLocation(int id)
        {
            var userLocation = await _context.UserLocation.FindAsync(id);

            if (userLocation == null)
            {
                return NotFound();
            }

            return userLocation;
        }

        // PUT: api/UserLocation/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserLocation(int id, UserLocation userLocation)
        {
            if (id != userLocation.UserLocationId)
            {
                return BadRequest();
            }

            _context.Entry(userLocation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserLocationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // should take in a list
        [HttpPost]
        public async Task<ActionResult<UserLocation>> PostUserLocation(UserLocation userLocation)
        {
            _context.UserLocation.Add(userLocation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserLocation", new { id = userLocation.UserLocationId }, userLocation);
        }

        // DELETE: api/UserLocation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserLocation>> DeleteUserLocation(int id)
        {
            var userLocation = await _context.UserLocation.FindAsync(id);
            if (userLocation == null)
            {
                return NotFound();
            }

            _context.UserLocation.Remove(userLocation);
            await _context.SaveChangesAsync();

            return userLocation;
        }

        private bool UserLocationExists(int id)
        {
            return _context.UserLocation.Any(e => e.UserLocationId == id);
        }
    }
}
