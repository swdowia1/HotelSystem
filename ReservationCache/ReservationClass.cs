using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReservationCache
{
    public class ReservationClass : IReservationCache
    {
        private readonly IDatabase _db;

        public ReservationClass(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<Reservation?> GetReservationAsync(string id)
        {
            var cached = await _db.StringGetAsync($"reservation:{id}");
            if (!cached.HasValue) return null;

            return JsonSerializer.Deserialize<Reservation>(cached);
        }

        public async Task SetReservationAsync(Reservation reservation, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(reservation);
            await _db.StringSetAsync($"reservation:{reservation.Id}", json, expiry);
        }
    } 
}
