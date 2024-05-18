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
using vesta_api.Database.Models.View.Requests;

namespace vesta_api.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Authorize(Roles = "admin")]
    public class TestQuestionsController(VestaContext context) : ControllerBase
    {
        // GET: api/TestQuestions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestQuestion>> GetTestQuestion(int id)
        {
            var testQuestion =
                await context.TestQuestions
                    .Include(question => question.TestQuestionAnswers)
                    .FirstOrDefaultAsync(question => question.Id == id);

            if (testQuestion == null)
            {
                return NotFound();
            }

            return testQuestion;
        }

        // PUT: api/TestQuestions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestQuestion(int id, EditTestQuestionRequest testQuestion)
        {
            if (id != testQuestion.Id)
            {
                return BadRequest();
            }

            var editingQuestion = await context.TestQuestions.FirstOrDefaultAsync(question => question.Id == id);

            if (editingQuestion == null) return NotFound();

            editingQuestion.Text = testQuestion.Text;
            editingQuestion.IsMultipleChoice = testQuestion.IsMultipleChoice;
            editingQuestion.IsActive = testQuestion.IsActive;

            context.Entry(editingQuestion).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/TestQuestions
        [HttpPost]
        public async Task<ActionResult<TestQuestion>> PostTestQuestion(CreateTestQuestionRequest testQuestion)
        {
            var newQuestion = new TestQuestion
            {
                Text = testQuestion.Text,
                IsMultipleChoice = testQuestion.IsMultipleChoice,
                IsActive = true,
                CategoryId = testQuestion.CategoryId,
            };

            context.TestQuestions.Add(newQuestion);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetTestQuestion", new { id = newQuestion.Id }, newQuestion);
        }

        private bool TestQuestionExists(int id)
        {
            return context.TestQuestions.Any(e => e.Id == id);
        }
    }
}