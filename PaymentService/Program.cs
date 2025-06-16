using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;

// Wczytaj konfigurację z appsettings.json
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Pobierz sekcję RabbitMQ.h,
var rabbitSettings = configuration.GetSection("RabbitMQ").Get<RabbitMQSettings>();

var factory = new ConnectionFactory()
{
    HostName = "rabbitmq",
    UserName = "user",       // dopasuj do docker-compose.yml
    Password = "password"    // dopasuj do docker-compose.yml
};

IConnection? connection = null;
IModel? channel = null;

int retries = 10;
while (retries > 0)
{
    try
    {
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        Console.WriteLine("✅ Connected to RabbitMQ");
        break;
    }
    catch (Exception ex)
    {
        retries--;
        Console.WriteLine($"⏳ RabbitMQ not ready. Retrying... ({retries} left)");
        Thread.Sleep(5000); // 5 sekund przerwy
    }
}

if (connection == null || channel == null)
{
    Console.WriteLine("❌ Could not connect to RabbitMQ after multiple attempts.");
    return;
}

// logika RabbitMQ
channel.ExchangeDeclare("reservation.exchange", ExchangeType.Fanout);
var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName, exchange: "reservation.exchange", routingKey: "");

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var evt = JsonSerializer.Deserialize<ReservationCreatedEvent>(message);
    Console.WriteLine($"[PaymentService] Received reservation: {evt.ReservationId}, {evt.Price} USD");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine("💸 PaymentService running...");
Console.ReadLine();

record ReservationCreatedEvent(int ReservationId, decimal Price, string GuestName);
public class RabbitMQSettings
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}