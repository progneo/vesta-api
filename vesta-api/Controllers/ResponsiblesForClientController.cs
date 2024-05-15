using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class ResponsiblesForClientController(VestaContext context) : ControllerBase
    {
        [HttpPost, Authorize(Roles = "admin,clientSpecialist")]
        public async Task<ActionResult<ResponsibleForClient>> PostResponsibleOfClient(
            ResponsibleForClientViewModel responsibleForClient)
        {
            context.ResponsibleForClients.Add(new ResponsibleForClient
            {
                ClientId = responsibleForClient.ClientId,
                ResponsibleId = responsibleForClient.ResponsibleId
            });
            await context.SaveChangesAsync();

            return Created();            
            
        }
    }
}