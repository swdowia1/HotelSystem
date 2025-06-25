namespace ReservationCache
{
    public class Reservation
    {
        public string Id { get; set; }
        public string GuestName { get; set; }
        public decimal Price { get; set; }
        public DateTime CheckIn { get; set; }
    }
}
