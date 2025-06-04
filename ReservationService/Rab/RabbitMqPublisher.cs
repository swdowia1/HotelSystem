using System.Text.Json;
using RabbitMQ.Client;
using System.Text;

namespace ReservationService.Rab
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
        }

        public void PublishReservationCreated(ReservationCreatedEvent evt)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare("reservation.exchange", ExchangeType.Fanout);

            var json = JsonSerializer.Serialize(evt);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish("reservation.exchange", "", null, body);
        }
    }

}
