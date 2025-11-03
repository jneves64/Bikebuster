using Microsoft.EntityFrameworkCore;
using BikeBuster.Models;
using Npgsql;
using System.Security.Cryptography.Xml;

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
            base.OnModelCreating(modelBuilder); 
            // Configuração do enum como string
            modelBuilder.Entity<UserModel>()
                .Property(u => u.DriverLicenseType)
                .HasConversion<string>();

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23505")
                {
                    var keyName = pgEx.ConstraintName ?? "UNKNOWN";

                    keyName = keyName
                        .Replace("IX_", "", StringComparison.OrdinalIgnoreCase)
                        .Replace("PK_", "ID ", StringComparison.OrdinalIgnoreCase);
                    throw new InvalidOperationException($"A {keyName.ToUpper()} entry with this value already exists");

                }
                throw;
            }
        }
    }
}


