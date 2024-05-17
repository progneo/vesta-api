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
    public class TestQuestionAnswersController : ControllerBase
    {
        private readonly VestaContext _context;

        public TestQuestionAnswersController(VestaContext context)
        {
            _context = context;
        }

        // GET: api/TestQuestionAnswers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestQuestionAnswer>>> GetTestQuestionAnswers()
        {
            return await _context.TestQuestionAnswers.ToListAsync();
        }

        // GET: api/TestQuestionAnswers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestQuestionAnswer>> GetTestQuestionAnswer(int id)
        {
            var testQuestionAnswer = await _context.TestQuestionAnswers.FindAsync(id);

            if (testQuestionAnswer == null)
            {
                return NotFound();
            }

            return testQuestionAnswer;
        }

        // PUT: api/TestQuestionAnswers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestQuestionAnswer(int id, TestQuestionAnswer testQuestionAnswer)
        {
            if (id != testQuestionAnswer.Id)
            {
                return BadRequest();
            }

            _context.Entry(testQuestionAnswer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionAnswerExists(id))
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

        // POST: api/TestQuestionAnswers
        [HttpPost]
        public async Task<ActionResult<TestQuestionAnswer>> PostTestQuestionAnswer(TestQuestionAnswer testQuestionAnswer)
        {
            _context.TestQuestionAnswers.Add(testQuestionAnswer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestQuestionAnswer", new { id = testQuestionAnswer.Id }, testQuestionAnswer);
        }

        // DELETE: api/TestQuestionAnswers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestQuestionAnswer(int id)
        {
            var testQuestionAnswer = await _context.TestQuestionAnswers.FindAsync(id);
            if (testQuestionAnswer == null)
            {
                return NotFound();
            }

            _context.TestQuestionAnswers.Remove(testQuestionAnswer);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestQuestionAnswerExists(int id)
        {
            return _context.TestQuestionAnswers.Any(e => e.Id == id);
        }
    }
}
