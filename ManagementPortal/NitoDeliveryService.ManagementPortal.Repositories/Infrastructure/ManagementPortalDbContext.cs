using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;

namespace NitoDeliveryService.ManagementPortal.Repositories.Infrastructure
{
    public class ManagementPortalDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Slot> Slots { get; set; }

        public ManagementPortalDbContext(ManagementPortalDbOptions options):base(GenerateOptions(options))
        {
            Database.EnsureCreated();
        }

        private static DbContextOptions<ManagementPortalDbContext> GenerateOptions(ManagementPortalDbOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManagementPortalDbContext>()
                .UseSqlServer(options.ConnectionString);
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Slot>()
                .Property(s => s.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Slots)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
