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
        public DbSet<NotificationModel> Notification { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ✅ Importante chamar o base

            // Configuração do enum como string
            modelBuilder.Entity<UserModel>()
                .Property(u => u.DriverLicenseType)
                .HasConversion<string>();

            // Aqui você pode adicionar outras configurações
            // Exemplo: índices, chaves compostas, relacionamentos, etc.
        }
    }
}

