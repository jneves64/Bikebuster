using Microsoft.EntityFrameworkCore;
using BikeBuster.Models;

namespace BikeBuster.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<BikeModel> Motos { get; set; }
        public DbSet<RiderModel> Entregadores { get; set; }
        public DbSet<RentalModel> Locacoes { get; set; }
    }
}

