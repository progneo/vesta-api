using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;

namespace vesta_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize]
    public class TestingController(VestaContext context) : ControllerBase
    {
        [HttpGet("new"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<TestQuestionCategory>>> GetTestingQuestions()
        {
            return await context.TestQuestionCategories
                .Where(c => c.IsActive)
                .Include(c => c.TestQuestions
                    .Where(q => q.IsActive)
                )
                .ThenInclude(q => q.TestQuestionAnswers.Where(
                    a => a.IsActive)
                )
                .ToListAsync();
        }

        // GET: api/Testing
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Testing>>> GetTestings()
        {
            return await context.Testings.ToListAsync();
        }

        // GET: api/Testing/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Testing>> GetTesting(int id)
        {
            var test = await context.Testings.FindAsync(id);

            if (test == null)
            {
                return NotFound();
            }

            return test;
        }

        // PUT: api/Testing/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPut("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> PutTesting(int id, Testing test)
        {
            if (id != test.Id)
            {
                return BadRequest();
            }

            context.Entry(test).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestingExists(id))
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

        // POST: api/Testing
        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Testing>> PostTesting(TestingViewModel test)
        {
            var newTesting = context.Testings.Add(new Testing
            {
                ClientId = test.ClientId,
            });
            await context.SaveChangesAsync();

            foreach (var answer in test.AnswerIds)
            {
                context.TestAnswerOfClients.Add(new TestAnswerOfClient
                {
                    TestingId = newTesting.Entity.Id,
                    AnswerId = answer
                });
            }

            await context.SaveChangesAsync();

            return CreatedAtAction("GetTesting", new { id = newTesting.Entity.Id }, newTesting.Entity);
        }


        // DELETE: api/Testing/5
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpDelete("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<IActionResult> DeleteTesting(int id)
        {
            var test = await context.Testings.FindAsync(id);
            if (test == null)
            {
                return NotFound();
            }

            context.Testings.Remove(test);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestingExists(int id)
        {
            return context.Testings.Any(e => e.Id == id);
        }
    }
}