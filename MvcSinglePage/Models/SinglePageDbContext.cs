using Microsoft.EntityFrameworkCore;
using MvcSinglePage.Models.DomainModels.ProductAggregates;

namespace MvcSinglePage.Models
{
    public class SinglePageDbContext : DbContext
    {
        public SinglePageDbContext(DbContextOptions options) : base(options)
        {
        }

        public SinglePageDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }

        public DbSet<Product> Product { get; set; }
    }
}
