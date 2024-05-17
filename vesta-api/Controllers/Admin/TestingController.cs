using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;

namespace vesta_api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin")]
    public class TestingController : ControllerBase
    {
        private readonly VestaContext _context;

        public TestingController(VestaContext context)
        {
            _context = context;
        }

        // GET: api/Testing
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Testing>>> GetTestings()
        {
            return await _context.Testings.ToListAsync();
        }

        // GET: api/Testing/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Testing>> GetTesting(int id)
        {
            var testing = await _context.Testings.FindAsync(id);

            if (testing == null)
            {
                return NotFound();
            }

            return testing;
        }

        // PUT: api/Testing/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTesting(int id, Testing testing)
        {
            if (id != testing.Id)
            {
                return BadRequest();
            }

            _context.Entry(testing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestingExists(id))
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

        // POST: api/Testing
        [HttpPost]
        public async Task<ActionResult<Testing>> PostTesting(Testing testing)
        {
            _context.Testings.Add(testing);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTesting", new { id = testing.Id }, testing);
        }

        // DELETE: api/Testing/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTesting(int id)
        {
            var testing = await _context.Testings.FindAsync(id);
            if (testing == null)
            {
                return NotFound();
            }

            _context.Testings.Remove(testing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestingExists(int id)
        {
            return _context.Testings.Any(e => e.Id == id);
        }
    }
}
