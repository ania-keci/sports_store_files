using Microsoft.EntityFrameworkCore;

namespace sports_store_files.Models {
	public class ApplicationDbContext : DbContext{
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

    }
}