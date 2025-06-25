namespace ReservationCache
{
    public interface IReservationCache
    {
        Task<Reservation?> GetReservationAsync(string id);
        Task SetReservationAsync(Reservation reservation, TimeSpan? expiry = null);
    }
}
