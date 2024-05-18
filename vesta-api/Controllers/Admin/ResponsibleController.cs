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
    public class ResponsibleController(VestaContext context) : ControllerBase
    {
        // GET: api/Responsible
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Responsible>>> GetResponsibles()
        {
            return await context.Responsibles.OrderBy(responsible => responsible.Id).ToListAsync();
        }

        // PUT: api/Responsible/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResponsible(int id, EditResponsibleRequest responsible)
        {
            if (id != responsible.Id)
            {
                return BadRequest();
            }

            var editingResponsible = await context.Responsibles.FirstOrDefaultAsync(r => r.Id == responsible.Id);

            if (editingResponsible == null) return NotFound();

            editingResponsible.FirstName = responsible.FirstName;
            editingResponsible.LastName = responsible.LastName;
            editingResponsible.Patronymic = responsible.Patronymic;
            editingResponsible.PhoneNumber = responsible.PhoneNumber;
            editingResponsible.Type = responsible.Type;
            
            context.Entry(editingResponsible).State = EntityState.Modified;

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

        private bool ResponsibleExists(int id)
        {
            return context.Responsibles.Any(e => e.Id == id);
        }
    }
}