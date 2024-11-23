using Microsoft.EntityFrameworkCore;

namespace Test.Models
{
    public class ApplicationDbCOntext:DbContext
    {
        public ApplicationDbCOntext(DbContextOptions options):base(options)
        {
                
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}
