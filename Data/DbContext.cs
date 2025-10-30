using Microsoft.EntityFrameworkCore;
using BikeBuster.Models;

namespace BikeBuster.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BikeModel> Bike { get; set; }
        public DbSet<UserModel> Rider { get; set; }
        public DbSet<RentalModel> Rent { get; set; }
    }
}

