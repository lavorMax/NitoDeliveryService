using Microsoft.EntityFrameworkCore;
using NitoDeliveryService.ManagementPortal.Entities.Entities;
using NitoDeliveryService.ManagementPortal.Services.Infrastructure;

namespace NitoDeliveryService.ManagementPortal.Repositories.Infrastructure
{
    public class ManagementPortalDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Slot> Slots { get; set; }
        public DbSet<ClientResponsible> ClientResponsibles { get; set; }
        public DbSet<ClientPhone> ClientPhones { get; set; }

        public ManagementPortalDbContext(ManagementPortalDbOptions options):base(GenerateOptions(options))
        {}

        private static DbContextOptions<ManagementPortalDbContext> GenerateOptions(ManagementPortalDbOptions options)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ManagementPortalDbContext>()
                .UseSqlServer(options.ConncectionString);
            return optionsBuilder.Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Slot>()
                .HasOne(s => s.Client)
                .WithMany(c => c.Slots)
                .HasForeignKey(s => s.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientResponsible>()
                .HasOne(cr => cr.Client)
                .WithMany(c => c.Responsibles)
                .HasForeignKey(cr => cr.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientPhone>()
                .HasOne(cp => cp.ClientResponsible)
                .WithMany(cr => cr.ClientPhones)
                .HasForeignKey(cp => cp.ClientResponsibleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
