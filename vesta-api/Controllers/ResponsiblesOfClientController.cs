using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsiblesOfClientController(VestaContext context) : ControllerBase
    {
        // GET: api/ResponsiblesOfClientController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsibleForClient>>> GetResponsibleOfClient()
        {
            return await context.ResponsibleForClients.ToListAsync();
        }

        // GET: api/ResponsiblesOfClientController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsibleForClient>> GetResponsibleOfClient(int id)
        {
            var adultOfClient = await context.ResponsibleForClients.FindAsync(id);

            if (adultOfClient == null)
            {
                return NotFound();
            }

            return adultOfClient;
        }

        // PUT: api/ResponsiblesOfClientController/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponsibleOfClient(int id, ResponsibleForClient responsibleForClient)
        {
            if (id != responsibleForClient.ClientId)
            {
                return BadRequest();
            }

            context.Entry(responsibleForClient).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponsibleOfClientExists(id))
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

        // POST: api/ResponsiblesOfClientController
        [HttpPost]
        public async Task<ActionResult<ResponsibleForClient>> PostResponsibleOfClient(
            ResponsibleForClient responsibleForClient)
        {
            context.ResponsibleForClients.Add(responsibleForClient);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResponsibleOfClientExists(responsibleForClient.ClientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetResponsibleOfClient", new { id = responsibleForClient.ClientId },
                responsibleForClient);
        }

        // DELETE: api/ResponsiblesOfClientController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponsibleOfClient(int id)
        {
            var adultOfClient = await context.ResponsibleForClients.FindAsync(id);
            if (adultOfClient == null)
            {
                return NotFound();
            }

            context.ResponsibleForClients.Remove(adultOfClient);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponsibleOfClientExists(int id)
        {
            return context.ResponsibleForClients.Any(e => e.ClientId == id);
        }
    }
}