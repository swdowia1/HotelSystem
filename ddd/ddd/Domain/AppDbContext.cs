using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ddd.Domain
{
    public class AppDbContext : DbContext
    {
        private readonly IMediator _mediator;
        public DbSet<Order> Orders => Set<Order>();

        public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // Zapis do bazy
            var result = await base.SaveChangesAsync(cancellationToken);

            // Publikuj eventy po commit
            await DispatchDomainEvents();

            return result;
        }
        private async Task DispatchDomainEvents()
        {
            // Pobierz encje z eventami
            var domainEntities = ChangeTracker
                .Entries<Order>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            // Wysyłaj eventy przez MediatR
            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }
        }
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
