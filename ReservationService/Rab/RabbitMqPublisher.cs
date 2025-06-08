using System.Text.Json;
using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Options;
using System.Runtime;

namespace ReservationService.Rab
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;

        public RabbitMqPublisher()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            // Mapuj sekcję RabbitMQ do obiektu
            var _settings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();
            var factory = new ConnectionFactory()
            {
                HostName = _settings.HostName,
                Port = _settings.Port,
                UserName = _settings.UserName,
                Password = _settings.Password
            };
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
