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
    public class TestQuestionCategoriesController(VestaContext context) : ControllerBase
    {
        // GET: api/TestQuestionCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestQuestionCategory>>> GetTestQuestionCategories()
        {
            return await context.TestQuestionCategories.OrderBy(category => category.Id).ToListAsync();
        }

        // GET: api/TestQuestionCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestQuestionCategory>> GetTestQuestionCategory(int id)
        {
            var testQuestionCategory = await context.TestQuestionCategories
                .Include(category => category.TestQuestions)
                .ThenInclude(question => question.TestQuestionAnswers)
                .FirstOrDefaultAsync(category => category.Id == id);

            if (testQuestionCategory == null)
            {
                return NotFound();
            }

            return testQuestionCategory;
        }

        // PUT: api/TestQuestionCategories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTestQuestionCategory(int id, TestQuestionCategory testQuestionCategory)
        {
            if (id != testQuestionCategory.Id)
            {
                return BadRequest();
            }

            context.Entry(testQuestionCategory).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestQuestionCategoryExists(id))
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

        // POST: api/TestQuestionCategories
        [HttpPost]
        public async Task<ActionResult<TestQuestionCategory>> PostTestQuestionCategory(
            TestQuestionCategory testQuestionCategory)
        {
            context.TestQuestionCategories.Add(testQuestionCategory);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetTestQuestionCategory", new { id = testQuestionCategory.Id },
                testQuestionCategory);
        }

        private bool TestQuestionCategoryExists(int id)
        {
            return context.TestQuestionCategories.Any(e => e.Id == id);
        }
    }
}