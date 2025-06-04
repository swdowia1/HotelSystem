using System.Collections.Generic;

namespace ReservationService.Model
{
    public class Reservation
    {
        public int Id { get; set; }
        public string GuestName { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }

}
