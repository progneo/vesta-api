using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;

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
        public async Task<ActionResult<Responsible>> PostResponsible(ResponsibleViewModel responsible)
        {
            var newResponsible = new Responsible
            {
                FirstName = responsible.FirstName,
                LastName = responsible.LastName,
                Patronymic = responsible.Patronymic,
                Type = responsible.Type,
                PhoneNumber = responsible.PhoneNumber,
                DocumentId = responsible.DocumentId
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