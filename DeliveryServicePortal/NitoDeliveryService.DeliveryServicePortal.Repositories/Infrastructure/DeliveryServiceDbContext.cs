using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.PlaceManagementPortal.Entities.Entities;
using System;

namespace NitoDeliveryService.PlaceManagementPortal.Repositories.Infrastructure
{
    public class DeliveryServiceDbContext : DbContext
    {
        public DbSet<PlaceView> Places { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<DishOrder> DishOrder { get; set; }
        public DbSet<Order> Order { get; set; }


        public DeliveryServiceDbContext(DeliveryServiceDbOptions options) : base(GenerateOptions(options))
        {
            Database.EnsureCreated();
        }

        private static DbContextOptions<DeliveryServiceDbContext> GenerateOptions(DeliveryServiceDbOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DeliveryServiceDbContext>()
                .UseSqlServer(options.ConnectionString);
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlaceView>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DishOrder>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
               .Property(c => c.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DishOrder>()
                .HasOne(d => d.Order)
                .WithMany(o => o.DishOrders)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
