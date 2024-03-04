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
    public class FathersController(VestaContext context) : ControllerBase
    {
        // GET: api/Fathers
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<IEnumerable<Father>>> GetFathers()
        {
            return await context.Fathers.ToListAsync();
        }

        // GET: api/Fathers/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Father>> GetFather(int id)
        {
            var father = await context.Fathers.FindAsync(id);

            if (father == null)
            {
                return NotFound();
            }

            return father;
        }

        // PUT: api/Fathers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> PutFather(int id, Father father)
        {
            if (id != father.Id)
            {
                return BadRequest();
            }

            context.Entry(father).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FatherExists(id))
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

        // POST: api/Fathers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Father>> PostFather(Father father)
        {
            context.Fathers.Add(father);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetFather", new { id = father.Id }, father);
        }

        // DELETE: api/Fathers/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> DeleteFather(int id)
        {
            var father = await context.Fathers.FindAsync(id);
            if (father == null)
            {
                return NotFound();
            }

            context.Fathers.Remove(father);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool FatherExists(int id)
        {
            return context.Fathers.Any(e => e.Id == id);
        }
    }
}