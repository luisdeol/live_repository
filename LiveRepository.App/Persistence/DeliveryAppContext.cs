using LiveRepository.App.Entities;
using Microsoft.EntityFrameworkCore;

namespace LiveRepository.App.Persistence
{
    public class DeliveryAppContext : DbContext
    {
        public DeliveryAppContext(DbContextOptions<DeliveryAppContext> options) : base(options) { }

        public DbSet<Delivery> Deliveries { get; set; }
        public DbSet<DeliveryItem> DeliveryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Delivery>().HasKey(d => d.Id);
            modelBuilder.Entity<Delivery>()
                .HasMany(d => d.Items)
                .WithOne()
                .HasForeignKey(di => di.DeliveryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<DeliveryItem>().HasKey(d => d.Id);
        }
    }
}
