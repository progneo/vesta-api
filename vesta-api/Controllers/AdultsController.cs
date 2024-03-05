using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class AdultsController(VestaContext context) : ControllerBase
    {
        // GET: api/Adults
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<IEnumerable<Adult>>> GetAdults()
        {
            return await context.Adults.ToListAsync();
        }

        // GET: api/Adults/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Adult>> GetAdult(int id)
        {
            var adult = await context.Adults.FindAsync(id);

            if (adult == null)
            {
                return NotFound();
            }

            return adult;
        }

        // PUT: api/Adults/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> PutAdult(int id, Adult adult)
        {
            if (id != adult.Id)
            {
                return BadRequest();
            }

            context.Entry(adult).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdultExists(id))
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

        // POST: api/Adults
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Adult>> PostAdult(Adult adult)
        {
            context.Adults.Add(adult);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetAdult", new { id = adult.Id }, adult);
        }

        // DELETE: api/Adults/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> DeleteAdult(int id)
        {
            var adult = await context.Adults.FindAsync(id);
            if (adult == null)
            {
                return NotFound();
            }

            context.Adults.Remove(adult);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdultExists(int id)
        {
            return context.Adults.Any(e => e.Id == id);
        }
    }
}