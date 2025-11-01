using Microsoft.EntityFrameworkCore;
using BikeBuster.Models;

namespace BikeBuster.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<BikeModel> Bike { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<RentalModel> Rent { get; set; }
    }
}

