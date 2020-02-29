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
    public class UserMessageController : ControllerBase
    {
        private readonly VisitContext _context;

        public UserMessageController(VisitContext context)
        {
            _context = context;
        }

        // GET: api/UserMessage
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserMessage>>> GetUserMessage()
        {
            return await _context.UserMessage.ToListAsync();
        }

        // GET: api/UserMessage/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserMessage>> GetUserMessage(int id)
        {
            var userMessage = await _context.UserMessage.FindAsync(id);

            if (userMessage == null)
            {
                return NotFound();
            }

            return userMessage;
        }

        // PUT: api/UserMessage/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserMessage(int id, UserMessage userMessage)
        {
            if (id != userMessage.UserMessageId)
            {
                return BadRequest();
            }

            _context.Entry(userMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserMessageExists(id))
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

        // POST: api/UserMessage
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<UserMessage>> PostUserMessage(UserMessage userMessage)
        {
            _context.UserMessage.Add(userMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserMessage", new { id = userMessage.UserMessageId }, userMessage);
        }

        // DELETE: api/UserMessage/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserMessage>> DeleteUserMessage(int id)
        {
            var userMessage = await _context.UserMessage.FindAsync(id);
            if (userMessage == null)
            {
                return NotFound();
            }

            _context.UserMessage.Remove(userMessage);
            await _context.SaveChangesAsync();

            return userMessage;
        }

        private bool UserMessageExists(int id)
        {
            return _context.UserMessage.Any(e => e.UserMessageId == id);
        }
    }
}
