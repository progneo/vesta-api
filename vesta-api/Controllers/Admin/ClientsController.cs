using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View.Requests;

namespace vesta_api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin")]
    public class ClientsController(VestaContext context) : ControllerBase
    {
        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await context.Clients.OrderBy(client => client.Id).ToListAsync();
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, EditClientRequest client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            var editedClient = await context.Clients.FirstOrDefaultAsync(c => c.Id == id);

            if (editedClient == null) return NotFound();
            
            editedClient.FirstName = client.FirstName;
            editedClient.LastName = client.LastName;
            editedClient.Patronymic = client.Patronymic;
            editedClient.BirthDate = client.BirthDate;
            editedClient.Address = client.Address;
            editedClient.DocumentId = client.DocumentId;
            editedClient.IsActive = client.IsActive;
            
            context.Entry(editedClient).State = EntityState.Modified;

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
        
        private bool ClientExists(int id)
        {
            return context.Clients.Any(e => e.Id == id);
        }
    }
}