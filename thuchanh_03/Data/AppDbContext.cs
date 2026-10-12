using Microsoft.EntityFrameworkCore;
using thuchanh_03.Models;

namespace thuchanh_03.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
}