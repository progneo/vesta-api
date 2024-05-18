using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class ResponsiblesForClientController(VestaContext context) : ControllerBase
    {
        [HttpPost, Authorize(Roles = "admin,clientSpecialist")]
        public async Task<ActionResult<ResponsibleForClient>> PostResponsibleOfClient(
            CreateResponsibleForClientRequest createResponsibleForClient)
        {
            context.ResponsibleForClients.Add(new ResponsibleForClient
            {
                ClientId = createResponsibleForClient.ClientId,
                ResponsibleId = createResponsibleForClient.ResponsibleId
            });
            await context.SaveChangesAsync();

            return Created();            
            
        }
    }
}