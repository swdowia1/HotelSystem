using Microsoft.EntityFrameworkCore;

namespace ddd.Domain
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(builder =>
            {
                // Konfiguracja Value Object: CustomerName
                builder.OwnsOne(o => o.CustomerName, cb =>
                {
                    cb.Property(cn => cn.Value)
                      .HasColumnName("CustomerName")
                      .IsRequired();
                });

                // Inne pola jeśli chcesz — np. daty, status itd.
                builder.Property(o => o.CreatedAt);
            });
        }
    }
}
