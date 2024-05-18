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
    public class TestQuestionAnswersController(VestaContext context) : ControllerBase
    {
        // PUT: api/TestQuestionAnswers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestQuestionAnswer(int id, EditTestQuestionAnswerRequest testQuestionAnswer)
        {
            if (id != testQuestionAnswer.Id)
            {
                return BadRequest();
            }

            var editingAnswer = await context.TestQuestionAnswers.FirstOrDefaultAsync(answer => answer.Id == id);

            if (editingAnswer == null) return NotFound();

            editingAnswer.Text = testQuestionAnswer.Text;
            editingAnswer.Score = testQuestionAnswer.Score;
            editingAnswer.IsActive = testQuestionAnswer.IsActive;

            context.Entry(editingAnswer).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionAnswerExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // POST: api/TestQuestionAnswers
        [HttpPost]
        public async Task<ActionResult<TestQuestionAnswer>> PostTestQuestionAnswer(
            CreateTestQuestionAnswerRequest answer)
        {
            var newAnswer = new TestQuestionAnswer
            {
                Text = answer.Text,
                Score = answer.Score,
                IsActive = true,
                QuestionId = answer.QuestionId,
            };

            context.TestQuestionAnswers.Add(newAnswer);
            await context.SaveChangesAsync();

            return Created();
        }

        private bool TestQuestionAnswerExists(int id)
        {
            return context.TestQuestionAnswers.Any(e => e.Id == id);
        }
    }
}