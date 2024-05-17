using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;

namespace vesta_api.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin")]
    public class ResponsibleController(VestaContext context) : ControllerBase
    {
        // GET: api/Responsible
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Responsible>>> GetResponsibles()
        {
            return await context.Responsibles.ToListAsync();
        }

        // GET: api/Responsible/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Responsible>> GetResponsible(int id)
        {
            var responsible = await context.Responsibles.FindAsync(id);

            if (responsible == null)
            {
                return NotFound();
            }

            return responsible;
        }

        // PUT: api/Responsible/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponsible(int id, Responsible responsible)
        {
            if (id != responsible.Id)
            {
                return BadRequest();
            }

            context.Entry(responsible).State = EntityState.Modified;

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

                throw;
            }

            return NoContent();
        }

        // POST: api/Responsible
        [HttpPost]
        public async Task<ActionResult<Responsible>> PostResponsible(Responsible responsible)
        {
            context.Responsibles.Add(responsible);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetResponsible", new { id = responsible.Id }, responsible);
        }

        // DELETE: api/Responsible/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponsible(int id)
        {
            var responsible = await context.Responsibles.FindAsync(id);
            if (responsible == null)
            {
                return NotFound();
            }

            context.Responsibles.Remove(responsible);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponsibleExists(int id)
        {
            return context.Responsibles.Any(e => e.Id == id);
        }
    }
}
