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
    public class ServicesController(VestaContext context) : ControllerBase
    {
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Service>>> GetServices()
        {
            return await context.Services.Where(s => s.IsActive).ToListAsync();
        }

        [HttpPut("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> PutService(int id, Service service)
        {
            if (id != service.Id)
            {
                return BadRequest();
            }

            context.Entry(service).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost, Authorize(Roles = "admin")]
        public async Task<ActionResult<Service>> PostService(Service service)
        {
            context.Services.Add(service);
            await context.SaveChangesAsync();

            return Created();
        }

        [HttpDelete("{id}"), Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteService(int id)
        {
            var service = await context.Services.FindAsync(id);
            if (service == null)
            {
                return NotFound();
            }

            context.Services.Remove(service);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ServiceExists(int id)
        {
            return context.Services.Any(e => e.Id == id);
        }
    }
}