using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;
using vesta_api.Database.Models.View.Requests;
using vesta_api.Database.Models.View.Responses;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class ClientsController(VestaContext context) : ControllerBase
    {
        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients(
            [FromQuery(Name = "firstName")] string? firstName,
            [FromQuery(Name = "lastName")] string? lastName,
            [FromQuery(Name = "patronymic")] string? patronymic,
            [FromQuery(Name = "birthDate")] string? birthDate
        )
        {
            var clients = await context.Clients.Where(c => c.IsActive == true).ToListAsync();

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

        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist,specialist,admin")]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await context.Clients
                .Where(c => c.IsActive == true)
                .Include(c => c.ResponsiblesForClient)!
                .ThenInclude(r => r.Responsible)
                .ThenInclude(r => r.Document)
                .Include(c => c.Notes
                    .Where(n => n.IsActive == true)
                )
                .Include(c => c.Testings
                    .OrderByDescending(t => t.Datetime)
                )
                .Include(c => c.Appointments
                    .OrderBy(a => a.Datetime)
                )
                .Include(c => c.Appointments)
                .ThenInclude(a => a.Employee)
                .Include(c => c.Appointments)
                .ThenInclude(a => a.Service)
                .Include(c => c.Document)
                .FirstOrDefaultAsync(c => c.Id == id);

            return client == null ? NotFound() : client;
        }

        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist,admin")]
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

                throw;
            }

            return NoContent();
        }

        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Client>> PostClient(CreateClientRequest createClient)
        {
            var newClient = context.Clients.Add(new Client
            {
                FirstName = createClient.FirstName,
                LastName = createClient.LastName,
                Patronymic = createClient.Patronymic,
                Sex = createClient.Sex,
                BirthDate = createClient.BirthDate,
                Address = createClient.Address,
                DocumentId = createClient.DocumentId,
                IsActive = true
            });
            await context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = newClient.Entity.Id }, newClient.Entity);
        }

        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            client.IsActive = false;

            context.Entry(client).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(int id)
        {
            return context.Clients.Any(e => e.Id == id);
        }
    }
}