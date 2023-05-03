using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure
{
    public class DeliveryServiceDbContext : DbContext
    {
        public DbSet<CategoryView> Categories { get; set; }
        public DbSet<PlaceView> Places { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DishOrder> DishOrder { get; set; }
        public DbSet<Order> Order { get; set; }


        public DeliveryServiceDbContext(DeliveryServiceDbOptions options) : base(GenerateOptions(options))
        {
        }

        private static DbContextOptions<DeliveryServiceDbContext> GenerateOptions(DeliveryServiceDbOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
                .UseSqlServer(options.ConncectionString);
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryView>()
                .HasOne(c => c.PlaceView)
                .WithMany(p => p.Categories)
                .HasForeignKey(c => c.PlaceView)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DishOrder>()
                .HasOne(d => d.Order)
                .WithMany(o => o.DishOrders)
                .HasForeignKey(d => d.Order)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
