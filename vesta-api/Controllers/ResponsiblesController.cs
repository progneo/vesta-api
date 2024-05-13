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
    public class ResponsiblesController(VestaContext context) : ControllerBase
    {
        // GET: api/Responsibles
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Responsible>>> GetResponsibles()
        {
            return await context.Responsibles.ToListAsync();
        }

        // GET: api/Responsibles/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Responsible>> GetResponsible(int id)
        {
            var adult = await context.Responsibles.FindAsync(id);

            if (adult == null)
            {
                return NotFound();
            }

            return adult;
        }

        // PUT: api/Responsibles/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> PutResponsible(int id, Responsible adult)
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
                if (!ResponsibleExists(id))
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

        // POST: api/Responsibles
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Responsible>> PostResponsible(Responsible adult)
        {
            context.Responsibles.Add(adult);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetResponsible", new { id = adult.Id }, adult);
        }

        // DELETE: api/Responsibles/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> DeleteResponsible(int id)
        {
            var adult = await context.Responsibles.FindAsync(id);
            if (adult == null)
            {
                return NotFound();
            }

            context.Responsibles.Remove(adult);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponsibleExists(int id)
        {
            return context.Responsibles.Any(e => e.Id == id);
        }
    }
}