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
    public class ClientsController(VestaContext context) : ControllerBase
    {
        // GET: api/Clients
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(
            [FromQuery(Name = "firstName")] string? firstName,
            [FromQuery(Name = "lastName")] string? lastName,
            [FromQuery(Name = "patronymic")] string? patronymic,
            [FromQuery(Name = "birthDate")] string? birthDate
        )
        {
            var clients = await context.Clients.ToListAsync();

            if (firstName != null)
            {
                clients = clients.Where(c => c.FirstName.Contains(firstName, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }

            if (lastName != null)
            {
                clients = clients.Where(c => c.LastName.Contains(lastName, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }

            if (patronymic != null)
            {
                clients = clients.Where(c =>
                        c.Patronymic.Contains(patronymic, StringComparison.CurrentCultureIgnoreCase))
                    .ToList();
            }

            if (birthDate != null)
            {
                clients = clients.Where(c => c.BirthDate.ToString("yyyy-MM-dd").Equals(birthDate)).ToList();
            }

            return clients;
        }

        // GET: api/Clients/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await context.Clients
                .Include(c => c.AdultsOfClient)!
                .ThenInclude(a => a.Adult)
                .Include(c => c.Notes)
                .Include(c => c.Tests)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Clients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            context.Entry(client).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // POST: api/Clients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost, Authorize(Roles = "clientSpecialist, admin")]
        public async Task<ActionResult<Client>> PostClient(ClientViewModel client)
        {
            var newClient = context.Clients.Add(new Client()
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Patronymic = client.Patronymic,
                Gender = client.Gender,
                BirthDate = client.BirthDate,
                Address = client.Address,
                IdentityDocument = client.IdentityDocument,
            });
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = newClient.Entity.Id }, newClient.Entity);
        }

        // DELETE: api/Clients/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist, admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            context.Clients.Remove(client);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return context.Clients.Any(e => e.Id == id);
        }
    }
}