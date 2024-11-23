using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Test.Models;

namespace Test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbCOntext dbCOntext;

        public ProductController(ApplicationDbCOntext dbCOntext)
        {
            this.dbCOntext = dbCOntext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(int page = 1, int pageSize = 10)
        {
            var products = await dbCOntext.Products
                .Include(p => p.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.ProductId,
                    p.ProductName,
                    p.CategoryId,
                    p.Category.CategoryName
                })
                .ToListAsync();

            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await dbCOntext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null) return NotFound();

            return Ok(new
            {
                product.ProductId,
                product.ProductName,
                product.CategoryId,
                product.Category.CategoryName
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            dbCOntext.Products.Add(product);
            await dbCOntext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProduct), new { id = product.ProductId }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.ProductId) return BadRequest();
            dbCOntext.Entry(product).State = EntityState.Modified;
            await dbCOntext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await dbCOntext.Products.FindAsync(id);
            if (product == null) return NotFound();
            dbCOntext.Products.Remove(product);
            await dbCOntext.SaveChangesAsync();
            return NoContent();
        }
    }
}
