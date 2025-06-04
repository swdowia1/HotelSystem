

using Microsoft.EntityFrameworkCore;

namespace ReservationService.Model
{
    public class ReservationDbContext : DbContext
    {
        public ReservationDbContext(DbContextOptions<ReservationDbContext> options) : base(options) { }
        public DbSet<Reservation> Reservations { get; set; }
    }

}
