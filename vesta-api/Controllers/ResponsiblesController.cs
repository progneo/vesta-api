using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;
using vesta_api.Database.Models.View.Requests;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class ResponsiblesController(VestaContext context) : ControllerBase
    {
        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Responsible>>> GetResponsibles()
        {
            return await context.Responsibles.ToListAsync();
        }

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

                throw;
            }

            return NoContent();
        }

        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Responsible>> PostResponsible(CreateResponsibleRequest createResponsible)
        {
            var existingResponsible = await
                context.Responsibles.FirstOrDefaultAsync(r => r.PhoneNumber == createResponsible.PhoneNumber);

            if (existingResponsible != null)
                return CreatedAtAction("GetResponsible", new { id = existingResponsible.Id }, existingResponsible);

            var newResponsible = new Responsible
            {
                FirstName = createResponsible.FirstName,
                LastName = createResponsible.LastName,
                Patronymic = createResponsible.Patronymic,
                Type = createResponsible.Type,
                PhoneNumber = createResponsible.PhoneNumber,
                DocumentId = createResponsible.DocumentId
            };

            context.Responsibles.Add(newResponsible);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetResponsible", new { id = newResponsible.Id }, newResponsible);
        }

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