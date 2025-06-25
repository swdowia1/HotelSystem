using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservationCache;
using ReservationService.Model;
using ReservationService.Rab;
using StackExchange.Redis;
using System.Text.Json;
using Reservation = ReservationService.Model.Reservation;
using ReservationCa = ReservationCache.Reservation;

namespace ReservationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly ReservationDbContext _dbContext;
        private readonly RabbitMqPublisher _publisher;
      //  private readonly IConnectionMultiplexer _redis;
        private readonly IReservationCache _rediscache;

        public ReservationsController(ReservationDbContext dbContext, IReservationCache rediscache)
        {
            _dbContext = dbContext;
            _publisher = new RabbitMqPublisher();
            _rediscache = rediscache;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
        {
            reservation.Status = "Created";
            _dbContext.Reservations.Add(reservation);

            await _dbContext.SaveChangesAsync();
            ReservationCa reservationCa = new ReservationCa();
            /*
              public string Id { get; set; }
            public string GuestName { get; set; }
            public decimal Price { get; set; }
            public DateTime CheckIn { get; set; }
             */
            reservationCa.Id=reservation.Id.ToString();
            reservationCa.GuestName = reservation.GuestName;
            reservationCa.CheckIn = reservation.CheckIn;
            _rediscache.SetReservationAsync(reservationCa);
          

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
           
           // var cached = await _rediscache.GetReservationAsync($"reservation:{id}");
            var cached = await _rediscache.GetReservationAsync("99");
            if (cached!=null)
            {
              
                return Ok(cached);
            }

            // pobierz z bazy danych (zak³adamy EF)
            var reservation = new Reservation { Id = id, GuestName = "Test", Price = 123, CheckIn = DateTime.Now };
 

            return Ok(reservation);
        }
    }

}
