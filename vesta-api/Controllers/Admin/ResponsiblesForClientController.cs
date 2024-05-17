using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;

namespace vesta_api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin")]
    public class ResponsiblesForClientController : ControllerBase
    {
        private readonly VestaContext _context;

        public ResponsiblesForClientController(VestaContext context)
        {
            _context = context;
        }

        // GET: api/ResponsiblesForClient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResponsibleForClient>>> GetResponsibleForClients()
        {
            return await _context.ResponsibleForClients.ToListAsync();
        }

        // GET: api/ResponsiblesForClient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ResponsibleForClient>> GetResponsibleForClient(int id)
        {
            var responsibleForClient = await _context.ResponsibleForClients.FindAsync(id);

            if (responsibleForClient == null)
            {
                return NotFound();
            }

            return responsibleForClient;
        }

        // PUT: api/ResponsiblesForClient/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponsibleForClient(int id, ResponsibleForClient responsibleForClient)
        {
            if (id != responsibleForClient.ClientId)
            {
                return BadRequest();
            }

            _context.Entry(responsibleForClient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResponsibleForClientExists(id))
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

        // POST: api/ResponsiblesForClient
        [HttpPost]
        public async Task<ActionResult<ResponsibleForClient>> PostResponsibleForClient(ResponsibleForClient responsibleForClient)
        {
            _context.ResponsibleForClients.Add(responsibleForClient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ResponsibleForClientExists(responsibleForClient.ClientId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetResponsibleForClient", new { id = responsibleForClient.ClientId }, responsibleForClient);
        }

        // DELETE: api/ResponsiblesForClient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResponsibleForClient(int id)
        {
            var responsibleForClient = await _context.ResponsibleForClients.FindAsync(id);
            if (responsibleForClient == null)
            {
                return NotFound();
            }

            _context.ResponsibleForClients.Remove(responsibleForClient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResponsibleForClientExists(int id)
        {
            return _context.ResponsibleForClients.Any(e => e.ClientId == id);
        }
    }
}
