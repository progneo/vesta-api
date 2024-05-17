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
    public class TestQuestionCategoriesController : ControllerBase
    {
        private readonly VestaContext _context;

        public TestQuestionCategoriesController(VestaContext context)
        {
            _context = context;
        }

        // GET: api/TestQuestionCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestQuestionCategory>>> GetTestQuestionCategories()
        {
            return await _context.TestQuestionCategories.ToListAsync();
        }

        // GET: api/TestQuestionCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestQuestionCategory>> GetTestQuestionCategory(int id)
        {
            var testQuestionCategory = await _context.TestQuestionCategories.FindAsync(id);

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

            _context.Entry(testQuestionCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
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
        public async Task<ActionResult<TestQuestionCategory>> PostTestQuestionCategory(TestQuestionCategory testQuestionCategory)
        {
            _context.TestQuestionCategories.Add(testQuestionCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTestQuestionCategory", new { id = testQuestionCategory.Id }, testQuestionCategory);
        }

        // DELETE: api/TestQuestionCategories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTestQuestionCategory(int id)
        {
            var testQuestionCategory = await _context.TestQuestionCategories.FindAsync(id);
            if (testQuestionCategory == null)
            {
                return NotFound();
            }

            _context.TestQuestionCategories.Remove(testQuestionCategory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TestQuestionCategoryExists(int id)
        {
            return _context.TestQuestionCategories.Any(e => e.Id == id);
        }
    }
}
