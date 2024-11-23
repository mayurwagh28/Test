using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbCOntext dbCOntext;

        public CategoryController(ApplicationDbCOntext dbCOntext)
        {
            this.dbCOntext = dbCOntext;
        }


        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await dbCOntext.Categories.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await dbCOntext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return Ok(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            dbCOntext.Categories.Add(category);
            await dbCOntext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryId) return BadRequest();
            dbCOntext.Entry(category).State = EntityState.Modified;
            await dbCOntext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await dbCOntext.Categories.FindAsync(id);
            if (category == null) return NotFound();
            dbCOntext.Categories.Remove(category);
            await dbCOntext.SaveChangesAsync();
            return NoContent();
        }


    }
}
