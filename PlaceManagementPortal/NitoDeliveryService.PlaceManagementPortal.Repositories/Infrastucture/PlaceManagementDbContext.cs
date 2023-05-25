using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class PlaceManagementDbContext : DbContext
    {
        public DbSet<Place> Places { get; set; }
        public DbSet<PaymentConfiguration> PaymentConfigurations { get; set; }
        public DbSet<Dish> Dishes { get; set; }

        public PlaceManagementDbContext(DbContextOptions<PlaceManagementDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Place>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PaymentConfiguration>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Dish>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<PaymentConfiguration>()
                .HasOne(pc => pc.Place)
                .WithMany(p => p.PaymentConfigurations)
                .HasForeignKey(pc => pc.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dish>()
                .HasOne(d => d.Place)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
