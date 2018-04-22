using Microsoft.EntityFrameworkCore;

namespace Dashboard.Models
{
    public class DashboardContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public DashboardContext(DbContextOptions<DashboardContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Auction> Auctions { get; set; }
    }
}