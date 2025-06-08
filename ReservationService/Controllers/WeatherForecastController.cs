using Microsoft.AspNetCore.Mvc;
using ReservationService.Model;
using ReservationService.Rab;
using StackExchange.Redis;
using System.Text.Json;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationDbContext _dbContext;
        private readonly RabbitMqPublisher _publisher;
        private readonly IConnectionMultiplexer _redis;

        public ReservationsController(ReservationDbContext dbContext, IConnectionMultiplexer redis)
        {
            _dbContext = dbContext;
            _publisher = new RabbitMqPublisher();
            _redis = redis;
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
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var db = _redis.GetDatabase();
            var cached = await db.StringGetAsync($"reservation:{id}");
            if (cached.HasValue)
            {
                var res = JsonSerializer.Deserialize<Reservation>(cached);
                return Ok(res);
            }

            // pobierz z bazy danych (zak³adamy EF)
            var reservation = new Reservation { Id = id, GuestName = "Test", Price = 123, CheckIn = DateTime.Now };
            await db.StringSetAsync($"reservation:{id}", JsonSerializer.Serialize(reservation), TimeSpan.FromMinutes(10));

            return Ok(reservation);
        }
    }

}
