using Microsoft.EntityFrameworkCore;
using SWIFTCARGOAPI.Models;


namespace SWIFTCARGOAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
    }
}