using Microsoft.EntityFrameworkCore;
using WebApplication.Models;

namespace WebApplication.DataContext
{
    public class ShopDbContext : DbContext
    {
        public ShopDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ShopDbContext()
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}