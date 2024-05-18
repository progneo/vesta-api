using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
    public class AppointmentsController(VestaContext context) : ControllerBase
    {
        [HttpGet("{date:datetime}"), Authorize(Roles = "specialist")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByDate(DateTime date)
        {
            var username = User.FindFirst(c => c.Type == ClaimsIdentity.DefaultNameClaimType)!.Value;
            var user = context.Users
                .Include(u => u.Employee)
                .First(u => u.Username == username);

            return await context.Appointments
                .Where(a => a.EmployeeId == user.EmployeeId && a.Datetime.Date == date.Date)
                .Include(a => a.Client)
                .Include(a => a.Service)
                .OrderBy(a => a.Datetime)
                .ToListAsync();
        }

        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByEmployeeId(
            [FromQuery(Name = "employeeId")] string? employeeId,
            [FromQuery(Name = "date")] string? date)
        {
            var appointments = await context.Appointments
                .Include(a => a.Client)
                .Include(a => a.Service)
                .OrderBy(a => a.Datetime)
                .ToListAsync();

            if (employeeId != null)
            {
                var user = context.Users
                    .Include(u => u.Employee)
                    .First(u => u.Employee.Id == int.Parse(employeeId));

                appointments = appointments
                    .Where(a => a.EmployeeId == user.EmployeeId)
                    .ToList();
            }

            if (date != null)
            {
                appointments = appointments
                    .Where(a => a.Datetime.Date.ToString("yyyy-MM-dd").Equals(date))
                    .ToList();
            }

            return appointments;
        }

        [HttpGet("Client/{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentByClientId(int id)
        {
            return await context.Appointments
                .Where(a => a.ClientId == id)
                .Include(a => a.Service)
                .OrderBy(a => a.Datetime)
                .ToListAsync();
        }

        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> PutAppointment(int id, Appointment appointment)
        {
            if (id != appointment.Id)
            {
                return BadRequest();
            }

            context.Entry(appointment).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppointmentExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Appointment>> PostAppointment(CreateAppointmentRequest createAppointment)
        {
            var newAppointment = new Appointment
            {
                Datetime = createAppointment.Datetime,
                ClientId = createAppointment.ClientId,
                EmployeeId = createAppointment.EmployeeId,
                ServiceId = createAppointment.ServiceId
            };
            
            context.Appointments.Add(newAppointment);
            await context.SaveChangesAsync();

            return Created();
        }

        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            var appointment = await context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            context.Appointments.Remove(appointment);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool AppointmentExists(int id)
        {
            return context.Appointments.Any(e => e.Id == id);
        }
    }
}