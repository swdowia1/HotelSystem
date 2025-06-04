namespace ReservationService.Rab
{
    public class ReservationCreatedEvent
    {
        public int ReservationId { get; set; }
        public decimal Price { get; set; }
        public string GuestName { get; set; }
    }

}
