using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

//var factory = new ConnectionFactory() { HostName = "rabbitmq" };
var factory = new ConnectionFactory() { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

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

Console.WriteLine("PaymentService running...");
Console.ReadLine();

record ReservationCreatedEvent(int ReservationId, decimal Price, string GuestName);
