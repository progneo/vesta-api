using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;
using vesta_api.Database.Models.View.Requests;
using vesta_api.Database.Models.View.Responses;

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

        [HttpGet, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<Testing>>> GetTestings()
        {
            return await context.Testings.ToListAsync();
        }

        [HttpGet("scores/{clientId}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<IEnumerable<ScoresByCategoryResponse>>> GetScoresOfTestingsByClientId(
            int clientId
        )
        {
            var testings = await context.Testings
                .Where(t => t.ClientId == clientId)
                .Include(t => t.TestAnswersOfClient)
                .ThenInclude(ta => ta.Answer)
                .ThenInclude(a => a.Question)
                .ThenInclude(q => q.Category)
                .ToListAsync();

            var scores = testings
                .SelectMany(t => t.TestAnswersOfClient, (t, ta) => new { t.Id, t.Datetime, ta })
                .GroupBy(ta => ta.ta.Answer.Question.Category.Name)
                .Select(g => new ScoresByCategoryResponse
                {
                    CategoryName = g.Key,
                    Scores = g.GroupBy(ta => ta.Id)
                        .Select(tg => new TestingScoreResponse
                        {
                            Id = tg.Key,
                            Score = tg.Sum(ta => ta.ta.Answer.Score),
                            Datetime = tg.First().Datetime
                        })
                        .ToList()
                });

            return scores.ToList();
        }

        [HttpGet("{id}"), Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<List<TestingCategoryQuestionsResponse>>> GetTesting(int id)
        {
            var test = await context.Testings
                .Include(t => t.TestAnswersOfClient)
                .ThenInclude(ta => ta.Answer)
                .ThenInclude(a => a.Question)
                .ThenInclude(tq => tq.Category)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (test == null)
            {
                return NotFound();
            }

            var response = test.TestAnswersOfClient
                .GroupBy(ta => ta.Answer.Question.Category.Name)
                .Select(g => new TestingCategoryQuestionsResponse
                {
                    Category = g.Key,
                    QuestionList = g
                        .GroupBy(ta => ta.Answer.QuestionId)
                        .Select(qg => new TestingQuestionAnswersResponse
                        {
                            Question = qg.First().Answer.Question.Text,
                            AnswerList = qg.Select(ta => ta.Answer.Text).ToList()
                        })
                        .ToList()
                })
                .ToList();

            return response;
        }

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

                throw;
            }

            return NoContent();
        }

        [HttpPost, Authorize(Roles = "clientSpecialist,admin")]
        public async Task<ActionResult<Testing>> PostTesting(CreateTestingAnswersOfClientRequest test)
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