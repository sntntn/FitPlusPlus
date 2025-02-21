using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace ChatService.API.Services
{
    public class RabbitMqPublisher
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" }; // Zavisi od Docker setup-a
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(exchange: "chat_events", type: ExchangeType.Fanout);
        }

        public void PublishEvent<T>(string eventType, T eventData)
        {
            var message = new { EventType = eventType, Data = eventData };
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

            _channel.BasicPublish(
                exchange: "chat_events",
                routingKey: "",
                basicProperties: null,
                body: body
            );
        }
    }
}