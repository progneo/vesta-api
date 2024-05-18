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
    public class NotesController(VestaContext context) : ControllerBase
    {
        [HttpGet, Authorize]
        public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
        {
            return await context.Notes.Where(n => n.IsActive == true).ToListAsync();
        }

        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            context.Entry(note).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        [HttpPost, Authorize]
        public async Task<ActionResult<Note>> PostNote(CreateNoteRequest createNote)
        {
            var newNote = new Note
            {
                Text = createNote.Text,
                ClientId = createNote.ClientId,
                IsActive = true
            };

            context.Notes.Add(newNote);
            await context.SaveChangesAsync();

            return Created();
        }

        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            note.IsActive = false;

            context.Entry(note).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return context.Notes.Any(e => e.Id == id);
        }
    }
}