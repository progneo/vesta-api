using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class DocumentsController(VestaContext context) : ControllerBase
    {
        // GET: api/Documents
        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Document>>> GetDocuments()
        {
            return await context.Documents.ToListAsync();
        }

        // GET: api/Documents/5
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Document>> GetDocument(int id)
        {
            var document = await context.Documents.FindAsync(id);

            if (document == null)
            {
                return NotFound();
            }

            return document;
        }

        // PUT: api/Documents/5
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> PutDocument(int id, Document document)
        {
            if (id != document.Id)
            {
                return BadRequest();
            }

            context.Entry(document).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DocumentExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/Documents
        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Document>> PostDocument(CreateDocumentRequest createDocument)
        {
            var newDocument = new Document()
            {
                Type = createDocument.Type,
                Series = createDocument.Series,
                Number = createDocument.Number
            };

            context.Documents.Add(newDocument);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetDocument", new { id = newDocument.Id }, newDocument);
        }

        // DELETE: api/Documents/5
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            var document = await context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            context.Documents.Remove(document);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocumentExists(int id)
        {
            return context.Documents.Any(e => e.Id == id);
        }
    }
}