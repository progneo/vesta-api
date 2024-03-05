using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdultsOfClientController(VestaContext context) : ControllerBase
    {
        // GET: api/AdultsOfClientController
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdultOfClient>>> GetAdultOfClient()
        {
            return await context.AdultOfClient.ToListAsync();
        }

        // GET: api/AdultsOfClientController/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AdultOfClient>> GetAdultOfClient(int id)
        {
            var adultOfClient = await context.AdultOfClient.FindAsync(id);

            if (adultOfClient == null)
            {
                return NotFound();
            }

            return adultOfClient;
        }

        // PUT: api/AdultsOfClientController/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdultOfClient(int id, AdultOfClient adultOfClient)
        {
            if (id != adultOfClient.ClientId)
            {
                return BadRequest();
            }

            context.Entry(adultOfClient).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdultOfClientExists(id))
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

        // POST: api/AdultsOfClientController
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AdultOfClient>> PostAdultOfClient(AdultOfClient adultOfClient)
        {
            context.AdultOfClient.Add(adultOfClient);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (AdultOfClientExists(adultOfClient.ClientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetAdultOfClient", new { id = adultOfClient.ClientId }, adultOfClient);
        }

        // DELETE: api/AdultsOfClientController/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdultOfClient(int id)
        {
            var adultOfClient = await context.AdultOfClient.FindAsync(id);
            if (adultOfClient == null)
            {
                return NotFound();
            }

            context.AdultOfClient.Remove(adultOfClient);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdultOfClientExists(int id)
        {
            return context.AdultOfClient.Any(e => e.ClientId == id);
        }
    }
}