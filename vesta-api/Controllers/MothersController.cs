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
    public class MothersController(VestaContext context) : ControllerBase
    {
        // GET: api/Mother
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<IEnumerable<Mother>>> GetMothers()
        {
            return await context.Mothers.ToListAsync();
        }

        // GET: api/Mother/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Mother>> GetMother(int id)
        {
            var mother = await context.Mothers.FindAsync(id);

            if (mother == null)
            {
                return NotFound();
            }

            return mother;
        }

        // PUT: api/Mother/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> PutMother(int id, Mother mother)
        {
            if (id != mother.Id)
            {
                return BadRequest();
            }

            context.Entry(mother).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotherExists(id))
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

        // POST: api/Mother
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Mother>> PostMother(Mother mother)
        {
            context.Mothers.Add(mother);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetMother", new { id = mother.Id }, mother);
        }

        // DELETE: api/Mother/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> DeleteMother(int id)
        {
            var mother = await context.Mothers.FindAsync(id);
            if (mother == null)
            {
                return NotFound();
            }

            context.Mothers.Remove(mother);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool MotherExists(int id)
        {
            return context.Mothers.Any(e => e.Id == id);
        }
    }
}