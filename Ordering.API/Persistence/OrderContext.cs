using Microsoft.EntityFrameworkCore;
using Ordering.API.Entities;

namespace Ordering.API.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tablo isimlerini küçük harf yaparak Postgres'in kafasını karıştırmıyoruz
            modelBuilder.Entity<Order>().ToTable("orders", "public"); // Küçük harf 'orders'

            base.OnModelCreating(modelBuilder);
        }
    }

}