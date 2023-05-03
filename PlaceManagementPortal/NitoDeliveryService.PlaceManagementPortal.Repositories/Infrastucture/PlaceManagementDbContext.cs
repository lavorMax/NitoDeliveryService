using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastucture
{
    public class PlaceManagementDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Place> Places { get; set; }
        public DbSet<PaymentConfiguration> PaymentConfigurations { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<PlaceCategory> PlaceCategories { get; set; }


        public PlaceManagementDbContext(DbContextOptions<PlaceManagementDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Place)
                .WithMany(p => p.Categories)
                .HasForeignKey(c => c.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PaymentConfiguration>()
                .HasOne(pc => pc.Place)
                .WithMany(p => p.PaymentConfigurations)
                .HasForeignKey(pc => pc.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Dish>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Dishes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlaceCategory>()
                .HasOne(d => d.Place)
                .WithMany(c => c.PlaceCategories)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
