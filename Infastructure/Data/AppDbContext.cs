using Domain.Entities.Catalog;
using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<Category> Categories { get; set; }

    }
}
