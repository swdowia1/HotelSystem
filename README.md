KROK 5: Uruchamianie
1. Uruchom RabbitMQ i SQL Server:

docker-compose up -d

2. Uruchom ReservationService z poziomu Visual Studio (F5)
3. Uruchom PaymentService (Console App)
🔎 Testowanie
1. Wejdź do Postmana:

POST http://localhost:{port}/api/reservations
Content-Type: application/json

{
  "guestName": "Jan Kowalski",
  "checkIn": "2025-07-01T14:00:00",
  "checkOut": "2025-07-05T12:00:00",
  "price": 1200.00
}
