using Microsoft.AspNetCore.Mvc;
using Test.Models;
using Microsoft.EntityFrameworkCore;


namespace Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbCOntext _context;  // Use underscore to denote the instance variable

        public HomeController(ApplicationDbCOntext context)
        {
            _context = context;  // Correctly assign the parameter to the instance variable
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            if (_context == null)
            {
                return NotFound();
            }

            var products = await _context.Products
                .Include(p => p.Category)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return View(products); // Pass the products data to the view
        }
    }
}
