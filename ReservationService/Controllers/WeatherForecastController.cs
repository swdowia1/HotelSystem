using Microsoft.AspNetCore.Mvc;
using ReservationService.Model;
using ReservationService.Rab;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationDbContext _dbContext;
        private readonly RabbitMqPublisher _publisher;

        public ReservationsController(ReservationDbContext dbContext)
        {
            _dbContext = dbContext;
            _publisher = new RabbitMqPublisher();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            reservation.Status = "Created";
            _dbContext.Reservations.Add(reservation);
            await _dbContext.SaveChangesAsync();

            var evt = new ReservationCreatedEvent
            {
                ReservationId = reservation.Id,
                Price = reservation.Price,
                GuestName = reservation.GuestName
            };

            _publisher.PublishReservationCreated(evt);
            return Ok(reservation);
        }
    }

}
