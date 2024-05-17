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
    public class TestQuestionsController : ControllerBase
    {
        private readonly VestaContext _context;

        public TestQuestionsController(VestaContext context)
        {
            _context = context;
        }

        // GET: api/TestQuestions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestQuestion>>> GetTestQuestions()
        {
            return await _context.TestQuestions.ToListAsync();
        }

        // GET: api/TestQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestQuestion>> GetTestQuestion(int id)
        {
            var testQuestion = await _context.TestQuestions.FindAsync(id);

            if (testQuestion == null)
            {
                return NotFound();
            }

            return testQuestion;
        }

        // PUT: api/TestQuestions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestQuestion(int id, TestQuestion testQuestion)
        {
            if (id != testQuestion.Id)
            {
                return BadRequest();
            }

            _context.Entry(testQuestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionExists(id))
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

        // POST: api/TestQuestions
        [HttpPost]
        public async Task<ActionResult<TestQuestion>> PostTestQuestion(TestQuestion testQuestion)
        {
            _context.TestQuestions.Add(testQuestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestQuestion", new { id = testQuestion.Id }, testQuestion);
        }

        // DELETE: api/TestQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestQuestion(int id)
        {
            var testQuestion = await _context.TestQuestions.FindAsync(id);
            if (testQuestion == null)
            {
                return NotFound();
            }

            _context.TestQuestions.Remove(testQuestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestQuestionExists(int id)
        {
            return _context.TestQuestions.Any(e => e.Id == id);
        }
    }
}
